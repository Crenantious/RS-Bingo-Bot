// <copyright file="EmptyTile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Board;

using SixLabors.ImageSharp;

public static class EmptyTile
{
    public static Image Create() =>
        BoardImages.EmptyTile;
}