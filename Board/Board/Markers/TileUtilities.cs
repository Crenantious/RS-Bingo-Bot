// <copyright file="EvidencePendingMarker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Board;

using RSBingo_Framework.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static Imaging.Board.BoardPreferences;
using static RSBingo_Common.General;
using static RSBingo_Common.Paths;

internal static class TileUtilities
{
    private static Image marker = BoardImages.EvidencePendingMarker;

    private static void MarkTile(Image tileImage)
    {
        Point markerPosition = new((tileImage.Width - marker.Width) / 2,
                                   (tileImage.Height - marker.Height) / 2);

        tileImage.Mutate(b => b.DrawImage(marker, markerPosition, 1));
    }

    public static Rectangle GetTileRect(this int tileIndex)
    {
        int x = BoardBorderPixelWidth + (BoardImages.TileSize.Width + TileGapX) * (tileIndex % TilesPerRow);
        int y = BoardBorderPixelHeight + (BoardImages.TileSize.Height + TileGapY) * (tileIndex / TilesPerColumn);
        return new(x, y, BoardImages.TileSize.Width, BoardImages.TileSize.Height);
    }

    public static Image GetTaskImage(BingoTask task) =>
        Image<Rgba32>.Load(GetTaskImagesResizedPath(task.Name));

    /// <summary>
    /// Places <paramref name="foreground"/> in the centre and on top of <paramref name="background"/>.
    /// </summary>
    public static void PlaceAtCentre(Image background, Image foreground)
    {
        int width = Math.Max(background.Width, foreground.Width);
        int height = Math.Max(background.Height, foreground.Height);

        background.Mutate(b => b.Pad(width, height));
        background.Mutate(b => b.DrawImage(foreground, 1));
    }

    public static void AddTaskToTile(Image tile, Image task)
    {
        Point taskPosition = new((tile.Width - task.Width) / 2 + TaskXOffsetPixels,
                                 (tile.Height - task.Height) / 2 + TaskYOffsetPixels);

        tile.Mutate(t => t.DrawImage(task, taskPosition, 1));
    }




}