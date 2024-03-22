// <copyright file="EvidencePendingTile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Board;

using RSBingo_Framework.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

public static class EvidencePendingTile
{
    public static Image Create(BingoTask task)
    {
        Image tile = BoardImages.TileBackground;
        Image border = BoardImages.TileBorder;
        Image taskImage = TileUtilities.GetTaskImage(task);
        Image marker = BoardImages.EvidencePendingMarker;

        tile.Mutate(i => i.Grayscale());
        taskImage.Mutate(i => i.Grayscale());

        TileUtilities.AddTaskToTile(tile, taskImage);
        TileUtilities.PlaceAtCentre(tile, border);
        TileUtilities.PlaceAtCentre(tile, marker);

        return tile;
    }
}