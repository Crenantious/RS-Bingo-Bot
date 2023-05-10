// <copyright file="BoardImage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging;

using RSBingo_Framework.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static RSBingo_Common.General;
using static RSBingoBot.Imaging.BoardPreferences;

internal static class BoardImage
{
    private static Image baseBoard;

    static BoardImage()
    {
        baseBoard = Image.Load(GetBaseBoardImagePath());
    }

    public static Image Create(Team team)
    {
        Image teamBoard = GetBoard(team);

        foreach (Tile tile in team.Tiles)
        {
            UpdateTile(teamBoard, tile);
        }

        return teamBoard;
    }

    public static Image UpdateTile(Image board, Tile tile)
    {
        Rectangle tileRect = GetTileRect(tile.BoardIndex);
        Point taskImagePosition = new(tileRect.Width / 2 + TaskXOffsetPixels, tileRect.Height / 2 + TaskYOffsetPixels);

        Image taskImage = GetTaskImage(tile.Task.Name, tileRect);
        Image tileImage = baseBoard.Clone(b => b.Crop(tileRect));
        tileImage.Mutate(t => t.DrawImage(taskImage, taskImagePosition, 1));

        board.Mutate(b => b.DrawImage(tileImage, new Point(tileRect.X, tileRect.Y), 1));

        return board;
    }

    public static Image UpdateTile(Tile tile)
    {
        string teamBoardPath = GetBoardImagePath(tile.Team.Name);
        Image teamBoard = Image<Rgba32>.Load(teamBoardPath);
        return UpdateTile(teamBoard, tile);
    }

    public static void ClearTile(Image board, Tile tile)
    {
        Rectangle tileRect = GetTileRect(tile.BoardIndex);
        Image tileImage = baseBoard.Clone(b => b.Crop(tileRect));
        board.Mutate(b => b.DrawImage(tileImage, new Point(tileRect.X, tileRect.Y), 1));
    }

    public static Image GetBoard(Team team)
    {
        string teamBoardPath = GetBoardImagePath(team.Name);
        return File.Exists(teamBoardPath) ? Image<Rgba32>.Load(teamBoardPath) : baseBoard.Clone(b => { });
    }

    private static Image GetTaskImage(string taskName, Rectangle tileRect)
    {
        // TODO: resize all images on startup
        Image taskImage = Image<Rgba32>.Load(GetTaskImagePath(taskName));
        Size size = new Size(tileRect.Width - TaskXPaddingPixels * 2, tileRect.Height - TaskYPaddingPixels * 2);
        taskImage.Mutate(i => i.Resize(size));
        return taskImage;
    }

    private static Rectangle GetTileRect(int tileIndex)
    {
        int x = BoardBorderPixelWidth + (TilePixelWidth + TileBorderPixelWidth) * tileIndex;
        int y = BoardBorderPixelHeight + (TilePixelHeight + TileBorderPixelHeight) * tileIndex;
        return new(x, y, TilePixelWidth, TilePixelHeight);
    }
}