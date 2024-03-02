// <copyright file="BoardUpdater.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging;

using RSBingo_Framework.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static RSBingo_Common.General;
using static RSBingo_Common.Paths;
using static RSBingoBot.Imaging.BoardPreferences;

public static class BoardUpdater
{
    private static BoardImages boardImages;

    static BoardUpdater()
    {
        boardImages = (BoardImages)General.DI.GetService(typeof(BoardImages))!;
    }

    internal static Image CreateEmptyBoard() =>
        boardImages.EmptyBoard.Clone(x => { });

    public static void UpdateTiles(this Board board, IEnumerable<(BingoTask? task, int boardIndex)> tasks)
    {
        foreach (var tile in tasks)
        {
            UpdateTile(board, tile.task, tile.boardIndex);
        }
    }

    public static void UpdateTile(this Board board, BingoTask? task, int boardIndex)
    {
        Rectangle tileRect = GetTileRect(boardIndex);

        Image tileImage = boardImages.EmptyBoard.Clone(b => b.Crop(tileRect));

        if (task is not null)
        {
            AddTaskToTile(GetTaskImage(task), tileImage);
        }

        board.Image.Mutate(b => b.DrawImage(tileImage, new Point(tileRect.X, tileRect.Y), 1));
    }

    private static void AddTaskToTile(Image task, Image tile)
    {
        Point taskPosition = new((tile.Width - task.Width) / 2 + TaskXOffsetPixels,
                                 (tile.Height - task.Height) / 2 + TaskYOffsetPixels);

        tile.Mutate(t => t.DrawImage(task, taskPosition, 1));
    }

    public static void MarkTileEvidencePending(this Board board, int boardIndex) =>
        MarkTile(board, boardIndex, boardImages.EvidencePendingMarker);

    public static void MarkTileComplete(this Board board, int boardIndex) =>
        MarkTile(board, boardIndex, boardImages.TileCompleteMarker);

    private static void MarkTile(this Board board, int boardIndex, Image marker)
    {
        Rectangle tileRect = GetTileRect(boardIndex);
        Point markerPosition = new(tileRect.X + (tileRect.Width - marker.Width) / 2,
            tileRect.Y + (tileRect.Height - marker.Height) / 2);

        board.Image.Mutate(b => b.DrawImage(marker, markerPosition, 1));
    }

    ///// <summary>
    ///// Gets the current Board for the <paramref name="team"/>. Or a blank one if it cannot be found.
    ///// </summary>
    ///// <returns>The path the Board is saved at.</returns>
    //public string SaveBoard()
    //{
    //    string path = GetTeamBoardPath(Name);
    //    FileStream fs = new(path, FileMode.Open);

    //    Board.SaveAsPng(fs);
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

    private static Image GetTaskImage(BingoTask task) =>
        Image<Rgba32>.Load(GetTaskImagesResizedPath(task.Name));

    private static Rectangle GetTileRect(this int tileIndex)
    {
        int x = BoardBorderPixelWidth + (TilePixelWidth + TileBorderPixelWidth) * (tileIndex % TilesPerRow);
        int y = BoardBorderPixelHeight + (TilePixelHeight + TileBorderPixelHeight) * (tileIndex / TilesPerColumn);
        return new(x, y, TilePixelWidth, TilePixelHeight);
    }
}