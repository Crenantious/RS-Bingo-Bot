// <copyright file="Board.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Board;

using SixLabors.ImageSharp;

public class Board
{
    public Image Image { get; internal set; } = null!;

    /// <summary>
    /// Used to determine what format to save as.
    /// </summary>
    public string FileExtension { get; set; } = ".png";

    internal Board()
    {

    }
}