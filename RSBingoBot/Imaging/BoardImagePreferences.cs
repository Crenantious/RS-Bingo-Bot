﻿// <copyright file="BoardPreferences.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging;

using SixLabors.Fonts;
using static RSBingo_Common.General;
using static RSBingo_Common.Paths;

internal static class BoardPreferences
{
    #region tile & borders

    /// <summary>
    /// Gets the width of each tile in pixels.
    /// </summary>
    public static int TilePixelWidth => 149;

    /// <summary>
    /// Gets the height of each tile in pixels.
    /// </summary>
    public static int TilePixelHeight => 149;

    /// <summary>
    /// Gets the width of the border between tiles.
    /// </summary>
    public static int TileBorderPixelWidth => 6;

    /// <summary>
    /// Gets the height of the border between tiles.
    /// </summary>
    public static int TileBorderPixelHeight => 6;

    /// <summary>
    /// Gets the width of the border surrounding the board.
    /// </summary>
    public static int BoardBorderPixelWidth => 3;

    /// <summary>
    /// Gets the height of the border surrounding the board.
    /// </summary>
    public static int BoardBorderPixelHeight => 3;

    #endregion

    #region task

    /// <summary>
    /// Gets the amount of pixels to move task images inside a tile from the centre, in the x direction.</br>
    /// Negative values moves the image left, positive moves right.
    /// </summary>
    public const int TaskXOffsetPixels = 0;

    /// <summary>
    /// Gets the amount of pixels to move task images inside a tile from the centre, in the y direction.</br>
    /// Negative values moves the image up, positive moves down.
    /// </summary>
    public const int TaskYOffsetPixels = 30;

    /// <summary>
    /// Gets the amount of pixels to pad task images inside a tile, in the x direction, on both sides.
    /// </summary>
    public const int TaskXPaddingPixels = 5;

    /// <summary>
    /// Gets the amount of pixels to pad task images inside a tile, in the y direction, on both sides.
    /// </summary>
    public const int TaskYPaddingPixels = 5;

    #endregion

    #region text

    /// <summary>
    /// The path of the font.
    /// </summary>
    public static string FontPath { get; } = FromResources("Fonts/Champagne & Limousines/Champagne & Limousines Bold.ttf");

    /// <summary>
    /// Gets the size of the font.
    /// </summary>
    public const int FontSize = 18;

    /// <summary>
    /// Gets the style of the font.
    /// </summary>
    public const FontStyle Style = FontStyle.Bold;

    /// <summary>
    /// Gets the amount of pixels to move the text from the top of the tile (additive with <see cref="TextYPaddingPixels"/>).
    /// </summary>
    public const int TextTopOffsetPixels = 5;

    /// <summary>
    /// Gets the amount of pixels to pad text inside a tile in the x direction.
    /// </summary>
    public const int TextXPaddingPixels = 3;

    /// <summary>
    /// Gets the amount of pixels to pad text inside a tile in the y direction (additive with <see cref="TextTopOffsetPixels"/>).
    /// </summary>
    public const int TextYPaddingPixels = 3;

    #endregion
}