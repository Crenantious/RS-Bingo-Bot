// <copyright file="BoardImage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging;

using RSBingo_Framework.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static RSBingo_Common.Paths;
using static RSBingo_Common.General;
using static RSBingoBot.Imaging.BoardPreferences;

public static class BoardImage
{
    private static Image boardBackground;
    private static Image tileCompletedMarker;

    static BoardImage()
    {
        boardBackground = Image.Load(BoardBackgroundPath);
        tileCompletedMarker = Image.Load(TileCompletedMarkerPath);
    }

    public static Image Create(Team team)
    {
        Image teamBoard = GetBoard(team);

        foreach (Tile tile in team.Tiles.OrderBy(t => t.BoardIndex))
        {
            UpdateTile(teamBoard, tile);
        }

        return teamBoard;
    }

    public static Image UpdateTile(Image board, Tile tile)
    {
        Rectangle tileRect = GetTileRect(tile.BoardIndex);
        Image taskImage = GetTaskImage(tile.Task.Name, tileRect);

        Point taskImagePosition = new((tileRect.Width - taskImage.Width) / 2 + TaskXOffsetPixels,
            (tileRect.Height - taskImage.Height) / 2 + TaskYOffsetPixels);
        Image tileImage = boardBackground.Clone(b => b.Crop(tileRect));

        tileImage.Mutate(t => t.DrawImage(taskImage, taskImagePosition, 1));

        board.Mutate(b => b.DrawImage(tileImage, new Point(tileRect.X, tileRect.Y), 1));

        return board;
    }

    public static Image UpdateTile(Tile tile)
    {
        string teamBoardPath = GetTeamBoardPath(tile.Team.Name);
        Image teamBoard = Image<Rgba32>.Load(teamBoardPath);
        return UpdateTile(teamBoard, tile);
    }

    public static void ClearTile(Image board, Tile tile)
    {
        Rectangle tileRect = GetTileRect(tile.BoardIndex);
        Image tileImage = boardBackground.Clone(b => b.Crop(tileRect));
        board.Mutate(b => b.DrawImage(tileImage, new Point(tileRect.X, tileRect.Y), 1));
    }

    public static void MarkTileComplete(Image board, Tile tile)
    {
        Rectangle tileRect = GetTileRect(tile.BoardIndex);
        Point markerPosition = new(tileRect.X + tileRect.Width / 2, tileRect.Y + tileRect.Height / 2);
        board.Mutate(b => b.DrawImage(tileCompletedMarker, markerPosition, 1));
    }

    public static Image GetBoard(Team team)
    {
        string teamBoardPath = GetTeamBoardPath(team.Name);
        return File.Exists(teamBoardPath) ? Image<Rgba32>.Load(teamBoardPath) : boardBackground.Clone(b => { });
    }

    private static Image GetTaskImage(string taskName, Rectangle tileRect)
    {
        // TODO: resize all images on startup
        Image taskImage = Image<Rgba32>.Load(GetTaskImagePath(taskName));

        int width = tileRect.Width - (TaskXPaddingPixels + (int)MathF.Abs(TaskXOffsetPixels)) * 2;
        int height = tileRect.Height - (TaskYPaddingPixels + (int)MathF.Abs(TaskYOffsetPixels)) * 2;

        ResizeOptions resizeOptions = new()
        {
            Size = new Size(width, height),
            Mode = ResizeMode.Max
        };
        taskImage.Mutate(i => i.Resize(resizeOptions));

        return taskImage;
    }

    private static Rectangle GetTileRect(int tileIndex)
    {
        int x = BoardBorderPixelWidth + (TilePixelWidth + TileBorderPixelWidth) * (tileIndex % TilesPerRow);
        int y = BoardBorderPixelHeight + (TilePixelHeight + TileBorderPixelHeight) * (tileIndex / TilesPerColumn);
        return new(x, y, TilePixelWidth, TilePixelHeight);
    }
}