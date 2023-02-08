// <copyright file="BoardImage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging;

using RSBingo_Framework;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using SixLabors.ImageSharp;
using static RSBingo_Framework.DAL.DataFactory;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Drawing.Processing;
using RSBingoBot.Exceptions;
using SixLabors.Fonts;
using static RSBingo_Common.General;
using static BoardPreferences;
using static RSBingo_Framework.Records.BingoTaskRecord;

public class BoardImage
{
    private const string boardBaseFileName = "Board base.png";
    private const string noTaskName = "No task";

    private static readonly IDataWorker DataWorker;
    private static readonly Dictionary<int, TaskInfo> tasksInfo = new();
    private static readonly List<Image> baseTiles = new();
    private static readonly Dictionary<int, Board> boards = new();
    private static readonly Font font;
    private static readonly TextOptions textOptions;
    private static readonly Image<Rgba32> emptyTaskImage = new(TilePixelWidth, TileBorderPixelHeight);

    private static Image<Rgba32> boardBase = null!;

    static BoardImage()
    {
        FontCollection collection = new();
        FontFamily family = collection.Add(FontPath);
        font = family.CreateFont(FontSize, Style);

        DataWorker = CreateDataWorker();
        DisectBoardBase();

        textOptions = new(font)
        {
            Origin = new PointF(baseTiles[0].Width / 2, TextTopOffsetPixels),
            WrappingLength = TilePixelWidth - TextXPaddingPixels,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Top,
            TextAlignment = TextAlignment.Center
        };
    }

    /// <summary>
    /// Creates all boards for existing teams and updates them in their channels. Currently just used to test how the board looks.
    /// </summary>
    /// <returns></returns>
    public static async Task CreateAndUpdateAllTeamBoards()
    {
        foreach (Team team in DataWorker.Teams.GetAll())
        {
            await DiscordTeam.UpdateBoard(team, CreateBoard(team));
        }
    }

    public static Image CreateBoard(Team team)
    {
        boards.Add(team.RowId, new(team));

        foreach (Tile tile in team.Tiles)
        {
            UpdateTile(team, tile.BoardIndex, tile.Task);
        }

        return boards[team.RowId].Image;
    }

    public static Image GetTeamBoard(Team team) =>
        boards.ContainsKey(team.RowId) ?
            boards[team.RowId].Image :
            CreateBoard(team);

    public static Image<Rgba32> UpdateTile(Team team, int tileIndex, BingoTask? task)
    {
        if (!boards.ContainsKey(team.RowId))
        {
            CreateBoard(team);
        }

        UpdateTaskInfo(team, tileIndex, task);
        UpdateBoardImage(boards[team.RowId], team, tileIndex);

        return boards[team.RowId].Image;
    }

    private static TaskInfo UpdateTaskInfo(Team team, int tileIndex, BingoTask? task)
    {
        TaskInfo taskInfo;

        if (task == null)
        {
            taskInfo = new();
        }
        else
        {
            GetTaskInfo(task);
            taskInfo = tasksInfo[task.RowId];
        }

        boards[team.RowId].Tiles[tileIndex].taskInfo = taskInfo;
        return taskInfo;
    }

    private static Image<Rgba32> UpdateBoardImage(Board board, Team team, int tileIndex)
    {
        TileInfo tileInfo = board.Tiles[tileIndex];
        TaskInfo taskInfo = tileInfo.taskInfo;

        Image tileImage = baseTiles[tileIndex]
            .Clone(i => i.DrawImage(taskInfo.Image, new Point(taskInfo.XPadding, taskInfo.YPadding), 1));

        DrawTaskNameOnImage(tileImage, taskInfo.Name);
        board.Image.Mutate(i => i.DrawImage(tileImage, new Point(tileInfo.x, tileInfo.y), 1));
        return board.Image;
    }

    private static void DrawTaskNameOnImage(Image image, string name) =>
        image.Mutate(i => i.DrawText(textOptions, name, Color.Black));

    private static void DisectBoardBase()
    {
        // TODO: JR - have the dissection info stored in either a config file or the DB, and have it be changed via a command.

        try
        {
            boardBase = Image.Load<Rgba32>(TaskTemplatePopulator.GetFilePath(boardBaseFileName));
        }
        catch
        {
            throw new BoardImageException($"Could not find the '{boardBaseFileName}' file. " +
                "Please ensure one exists in order to create the team boards.");
        }

        CheckBoardBaseDimensions(boardBase);

        for (int i = 0; i < TilesPerColumn; i++)
        {
            for (int j = 0; j < TilesPerRow; j++)
            {
                int xPos = GetTileXPosition(j);
                int yPos = GetTileYPosition(i);
                var rectangle = new Rectangle(xPos, yPos, TilePixelWidth, TilePixelHeight);
                baseTiles.Add(boardBase.Clone(i => i.Crop(rectangle)));
            }
        }
    }

    private static void CheckBoardBaseDimensions(Image boardBase)
    {
        int expectedBoardBaseWidth = BoardBorderPixelWidth * 2 +
            TilePixelWidth * TilesPerRow +
            TileBorderPixelWidth * (TilesPerRow - 1);

        if (boardBase.Width != expectedBoardBaseWidth)
        {
            throw new BoardImageException($"Incorrect board base width. " +
                $"Expected {expectedBoardBaseWidth} but was {boardBase.Width}.");
        }

        int expectedBoardBaseHeight = BoardBorderPixelHeight * 2 +
            TilePixelHeight * TilesPerColumn +
            TileBorderPixelHeight * (TilesPerColumn - 1);

        if (boardBase.Height != expectedBoardBaseHeight)
        {
            throw new BoardImageException($"Incorrect board base height. " +
                $"Expected {expectedBoardBaseHeight} but was {boardBase.Height}.");
        }
    }

    private static TaskInfo GetTaskInfo(BingoTask? task)
    {
        if (task == null) { return new(); }

        // Check if it's already loaded.
        if (tasksInfo.ContainsKey(task.RowId)) { return tasksInfo[task.RowId]; }

        Image image;
        string imagePath = GetTaskImagePath(task.Name);
        string taskName = task != null ? task.Name : noTaskName;
        int taskId = task != null ? task.RowId : -1;

        try
        {
            image = Image.Load<Rgba32>(imagePath);
        }
        catch
        {
            LoggingLog($"Could not load the {task!.Name} task image, it may have been corrupted; moved or deleted.");
            image = emptyTaskImage;
            taskName = "Unable to find image";
        }

        var resizeOptions = new ResizeOptions()
        {
            Mode = ResizeMode.Max,
            Size = new Size(
                TilePixelWidth - TaskXPaddingPixels * 2 - (int)MathF.Abs(TaskXOffsetPixels),
                TilePixelHeight - TaskYPaddingPixels * 2 - (int)MathF.Abs(TaskYOffsetPixels))
        };

        image.Mutate(i => i.Resize(resizeOptions));

        int x = (TilePixelWidth - image.Width + TaskXOffsetPixels) / 2;
        int y = (TilePixelHeight - image.Height + TaskYOffsetPixels) / 2;

        TaskInfo taskInfo = new(image, taskName, x, y);
        tasksInfo.Add(taskId, taskInfo);
        return taskInfo;
    }

    private static int GetTileXPosition(int tileRowIndex) =>
        BoardBorderPixelWidth + (TilePixelWidth + TileBorderPixelWidth) * tileRowIndex;

    private static int GetTileYPosition(int tileColumnIndex) =>
        BoardBorderPixelHeight + (TilePixelHeight + TileBorderPixelHeight) * tileColumnIndex;

    private class TaskInfo
    {
        public Image Image { get; }
        public string Name { get; }
        public int XPadding { get; }
        public int YPadding { get; }

        public TaskInfo()
        {
            Image = emptyTaskImage;
            Name = noTaskName;
            XPadding = 0;
            YPadding = 0;
        }

        public TaskInfo(Image image, string name, int xPadding, int yPadding)
        {
            Image = image;
            Name = name;
            XPadding = xPadding;
            YPadding = yPadding;
        }
    }

    private class TileInfo
    {
        public TaskInfo taskInfo { get; set; }
        public int baseTileIndex { get; }
        public int x { get; }
        public int y { get; }

        public TileInfo(TaskInfo taskInfo, int baseTileIndex, int x, int y)
        {
            this.taskInfo = taskInfo;
            this.baseTileIndex = baseTileIndex;
            this.x = x;
            this.y = y;
        }
    }

    private class Board
    {
        public Image<Rgba32> Image = boardBase.Clone();
        public TileInfo[] Tiles = new TileInfo[MaxTilesOnABoard];

        public Board(Team team)
        {
            for (int i = 0; i < MaxTilesOnABoard; i++)
            {
                Tiles[i] = new(new TaskInfo(), i, GetTileXPosition(i % TilesPerRow), GetTileYPosition(i / TilesPerColumn));
                UpdateBoardImage(this, team, i);
            }
        }
    }
}