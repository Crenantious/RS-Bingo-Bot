// <copyright file="EvidencePendingTileFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Board;

using SixLabors.ImageSharp;

public class EvidencePendingTileFactory : TileWithMarkerFactory
{
    protected override Image Marker { get; set; } = BoardImages.EvidencePendingMarker;
}