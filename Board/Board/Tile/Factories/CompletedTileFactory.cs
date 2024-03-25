// <copyright file="CompletedTileFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Board;

using SixLabors.ImageSharp;

public class CompletedTileFactory : TileWithMarkerFactory
{
    protected override Image Marker { get; set; } = BoardImages.TileCompleteMarker;
}