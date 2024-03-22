// <copyright file="NoMarkerTile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Board;

using RSBingo_Framework.Models;
using SixLabors.ImageSharp;

public static class NoMarkerTile
{
    public static Image Create(BingoTask task)
    {
        Image tile = BoardImages.EmptyTile;
        Image taskImage = TileUtilities.GetTaskImage(task);

        TileUtilities.AddTaskToTile(tile, taskImage);

        return tile;
    }
}