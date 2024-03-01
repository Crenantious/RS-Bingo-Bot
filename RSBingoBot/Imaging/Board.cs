// <copyright file="Board.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging;

using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static RSBingo_Common.General;
using static RSBingo_Common.Paths;
using static RSBingoBot.Imaging.BoardPreferences;

public class Board : IBoard
{
    private static Image boardBackground = null!;
    private static Image emptyTask = null!;
    private static Image tileCompletedMarker = null!;
    private static Image evidencePendingMarker = null!;

    private Image board;

    static Board()
    {
        IDataWorker dataWorker = DataFactory.CreateDataWorker();
        boardBackground = Image.Load(BoardBackgroundPath);
        emptyTask = Image.Load(EmptyTaskPath);
        tileCompletedMarker = GetResizedMarker(TileCompletedMarkerPath);
        evidencePendingMarker = GetResizedMarker(EvidencePendingMarkerPath);

        // TODO: JR - move to a dedicated class that runs when new tasks get added/set.
        ResizeTaskImages(dataWorker);
    }

    public Board()
    {
        board = boardBackground.Clone(x => { });
    }

    public void UpdateTiles(IEnumerable<(BingoTask? task, int boardIndex)> tasks)
    {
        foreach (var tile in tasks)
        {
            UpdateTile(tile.task, tile.boardIndex);
        }
    }

    public void UpdateTile(BingoTask? task, int boardIndex)
    {
        Rectangle tileRect = GetTileRect(boardIndex);

        Image taskImage = task is null ?
            emptyTask :
            Image<Rgba32>.Load(GetTaskImagesResizedPath(task.Name));

        Point taskImagePosition = new((tileRect.Width - taskImage.Width) / 2 + TaskXOffsetPixels,
            (tileRect.Height - taskImage.Height) / 2 + TaskYOffsetPixels);
        Image tileImage = boardBackground.Clone(b => b.Crop(tileRect));

        tileImage.Mutate(t => t.DrawImage(taskImage, taskImagePosition, 1));

        board.Mutate(b => b.DrawImage(tileImage, new Point(tileRect.X, tileRect.Y), 1));
    }

    public void MarkTileEvidencePending(int boardIndex) =>
        MarkTile(boardIndex, evidencePendingMarker);

    public void MarkTileComplete(int boardIndex) =>
        MarkTile(boardIndex, tileCompletedMarker);

    private void MarkTile(int boardIndex, Image marker)
    {
        Rectangle tileRect = GetTileRect(boardIndex);
        Point markerPosition = new(tileRect.X + (tileRect.Width - marker.Width) / 2,
            tileRect.Y + (tileRect.Height - marker.Height) / 2);

        board.Mutate(b => b.DrawImage(marker, markerPosition, 1));
    }

    ///// <summary>
    ///// Gets the current board for the <paramref name="team"/>. Or a blank one if it cannot be found.
    ///// </summary>
    ///// <returns>The path the board is saved at.</returns>
    //public string SaveBoard()
    //{
    //    string path = GetTeamBoardPath(Name);
    //    FileStream fs = new(path, FileMode.Open);

    //    board.SaveAsPng(fs);
    //    fs.Close();
    //    return path;
    //}

    //public void Rename(string newName)
    //{
    //    string teamBoardPath = GetTeamBoardPath(Name);
    //    if (File.Exists(teamBoardPath))
    //    {
    //        File.Move(teamBoardPath, GetTeamBoardPath(newName));
    //    }
    //}

    private static Image GetResizedMarker(string path)
    {
        Image marker = Image<Rgba32>.Load(path);
        int width = TilePixelWidth - MarkerXPaddingPixels * 2;
        int height = TilePixelHeight - MarkerYPaddingPixels * 2;
        ResizeImageForTile(marker, width, height);
        return marker;
    }

    private static void ResizeImageForTile(Image taskImage, int width, int height)
    {
        ResizeOptions resizeOptions = new()
        {
            Size = new Size(width, height),
            Mode = ResizeMode.Max
        };
        taskImage.Mutate(i => i.Resize(resizeOptions));
    }

    private static void ResizeTaskImages(IDataWorker dataWorker)
    {
        foreach (BingoTask task in dataWorker.BingoTasks.GetAll())
        {
            Image taskImage = Image<Rgba32>.Load(GetTaskImagePath(task.Name));
            int width = TilePixelWidth - (TaskXPaddingPixels + (int)MathF.Abs(TaskXOffsetPixels)) * 2;
            int height = TilePixelHeight - (TaskYPaddingPixels + (int)MathF.Abs(TaskYOffsetPixels)) * 2;
            ResizeImageForTile(taskImage, width, height);
            taskImage.Save(GetTaskImagesResizedPath(task.Name));
        }
    }

    private static Rectangle GetTileRect(int tileIndex)
    {
        int x = BoardBorderPixelWidth + (TilePixelWidth + TileBorderPixelWidth) * (tileIndex % TilesPerRow);
        int y = BoardBorderPixelHeight + (TilePixelHeight + TileBorderPixelHeight) * (tileIndex / TilesPerColumn);
        return new(x, y, TilePixelWidth, TilePixelHeight);
    }
}