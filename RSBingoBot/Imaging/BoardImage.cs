// <copyright file="BoardImage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging
{
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using SixLabors.ImageSharp;
    using static RSBingo_Framework.DAL.DataFactory;
    using SixLabors.ImageSharp.Processing;
    using RSBingo_Framework;
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
        private static readonly IDataWorker DataWorker;
        private static Image<Rgba32> boardBase;
        private static readonly Dictionary<int, TaskInfo> tasksInfo = new();
        private static readonly List<Image> baseTiles = new();
        private static readonly Dictionary<int, Image<Rgba32>> teamBoards = new();
        private static readonly Dictionary<int, TileInfo> tilesInfo = new();
        private static readonly Font font;

        static BoardImage()
        {
            FontCollection collection = new();
            FontFamily family = collection.Add(FontPath);
            font = family.CreateFont(FontSize, Style);

            DataWorker = CreateDataWorker();
            DisectBoardBase();
        }

        /// <summary>
        /// Creates all boards for existing teams and updates them in their channels. Currently just used to test how the board looks.
        /// </summary>
        /// <returns></returns>
        public static async Task CreateAndUpdateAllTeamBoards()
        {
            foreach(Team team in DataWorker.Teams.GetAll())
            {
                
                await InitialiseTeam.UpdateBoard(team, CreateBoard(team));
            }
        }

        public static Image CreateBoard(Team team)
        {
            teamBoards.Add(team.RowId, boardBase.Clone());

            for (int i = 0; i < team.Tiles.Count(); i++)
            {
                Tile tile = team.Tiles.ElementAt(i);
                tilesInfo[tile.RowId] = new TileInfo(i,
                    GetTileXPosition(i % TilesPerRow),
                    GetTileYPosition(i / TilesPerColumn));
                UpdateTileTask(tile);
            }

            return teamBoards[team.RowId];
        }

        public static Image<Rgba32> UpdateTileTask(Tile tile)
        {
            if (!tilesInfo.ContainsKey(tile.RowId))
            {
                CreateBoard(tile.Team);
            }

            LoadImage(tile.Task);

            TileInfo tileInfo = tilesInfo[tile.RowId];
            TaskInfo taskInfo = tasksInfo[tile.TaskId];

            Image baseTileWithTask = baseTiles[tileInfo.baseTileIndex]
                .Clone(i => i.DrawImage(taskInfo.image, new Point(taskInfo.xPadding, taskInfo.yPadding), 1));

            TextOptions options = new(font)
            {
                Origin = new PointF(baseTileWithTask.Width / 2, TextTopOffsetPixels),
                WrappingLength = TilePixelWidth - TextXPaddingPixels,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                TextAlignment = TextAlignment.Center
            };

            baseTileWithTask.Mutate(
                i => i.DrawText(options, tile.Task.Name, Color.Black));

            teamBoards[tile.TeamId].Mutate(i => i.DrawImage(baseTileWithTask, new Point(tileInfo.x, tileInfo.y), 1));
            return teamBoards[tile.TeamId];
        }

        public static Image<Rgba32> GetBaseBoard() =>
            boardBase;

        public static Image<Rgba32> GetTeamBoard(int teamId) =>
            teamBoards[teamId];

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

        private static void LoadImage(BingoTask task)
        {
            // Check if it's already loaded.
            if (tasksInfo.ContainsKey(task.RowId)) { return; }

            Image image;
            string imagePath = GetTaskImagePath(task.Name);

            try
            {
                image = Image.Load<Rgba32>(imagePath);
            }
            catch
            {
                if (task.IsNoTask())
                {
                    image = new Image<Rgba32>(TilePixelWidth, TilePixelHeight);
                }
                else
                {
                    throw new BoardImageException($"Could not load the {task.Name} task image, it may have been corrupted or deleted.");
                }
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

            TaskInfo taskInfo = new(image, x, y);
            tasksInfo.Add(task.RowId, taskInfo);
        }

        private static int GetTileXPosition(int tileRowIndex) =>
            BoardBorderPixelWidth + (TilePixelWidth + TileBorderPixelWidth) * tileRowIndex;

        private static int GetTileYPosition(int tileColumnIndex) =>
            BoardBorderPixelHeight + (TilePixelHeight + TileBorderPixelHeight) * tileColumnIndex;

        private struct TaskInfo
        {
            public Image image { get; }
            public int xPadding { get; }
            public int yPadding { get; }

            public TaskInfo(Image image, int xPadding, int yPadding)
            {
                this.image = image;
                this.xPadding = xPadding;
                this.yPadding = yPadding;
            }
        }

        private struct TileInfo
        {
            public int baseTileIndex { get; }
            public int x { get; }
            public int y { get; }

            public TileInfo(int baseTileIndex, int xPadding, int yPadding)
            {
                this.baseTileIndex = baseTileIndex;
                this.x = xPadding;
                this.y = yPadding;
            }
        }
    }
}
