// <copyright file="CompletedTileFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Board;

using RSBingo_Framework.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

public abstract class TileWithMarkerFactory : TileFactoryBase<BingoTask>
{
    protected abstract Image Marker { get; set; }

    public override Image Create(BingoTask task)
    {
        Image tile = BoardImages.TileBackground;
        Image border = BoardImages.TileBorder;
        Image taskImage = TileUtilities.GetTaskImage(task);

        tile.Mutate(i => i.Grayscale());
        taskImage.Mutate(i => i.Grayscale());

        TileUtilities.AddTaskToTile(tile, taskImage);
        TileUtilities.PlaceAtCentre(tile, border);
        TileUtilities.PlaceAtCentre(tile, Marker);

        return tile;
    }
}