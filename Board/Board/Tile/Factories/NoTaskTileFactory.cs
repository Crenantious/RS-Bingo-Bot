// <copyright file="NoTaskTileFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Board;

using SixLabors.ImageSharp;

public class NoTaskTileFactory : TileFactoryBase
{
    public override Image Create() =>
        BoardImages.EmptyTile;
}