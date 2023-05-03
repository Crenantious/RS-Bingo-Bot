// <copyright file="GridImageUtilities.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging;

using RSBingoBot.DTO;
using RSBingoBot.Leaderboard;
using SixLabors.Fonts;

internal static class GridImageUtilities
{
    /// <summary>
    /// Calculates the dimensions of a grid image based on the grid, font, and optional padding.
    /// </summary>
    /// <param name="grid">The grid object containing the cells with text.</param>
    /// <param name="font">The font used for the text in the grid cells.</param>
    /// <param name="xPadding">Horizontal padding for the grid cells.</param>
    /// <param name="yPadding">Vertical padding for the grid cells.</param>
    /// <returns>The calculated dimensions.</returns>
    public static GridImageDimensions GetGridImageDimensions(Grid grid, Font font, int xPadding = 0, int yPadding = 0)
    {
        float[] columnWidths = new float[grid.Cells.GetLength(0)];
        float[] rowHeights = new float[grid.Cells.GetLength(1)];

        for (int i = 0; i < columnWidths.Length; i++)
        {
            for (int j = 0; j < rowHeights.Length; j++)
            {
                (float width, float height) = GetTextSize(grid.Cells[i, j], font);
                columnWidths[i] = MathF.Max(columnWidths[i], width);
                rowHeights[j] = MathF.Max(rowHeights[j], height);
            }
        }

        IEnumerable<int> widths = columnWidths.Select(w => (int)MathF.Ceiling(w) + xPadding);
        IEnumerable<int> heights = rowHeights.Select(h => (int)MathF.Ceiling(h) + yPadding);

        return new(widths, heights);
    }

    private static (float width, float height) GetTextSize(string name, Font font)
    {
        FontRectangle fontRectangle = TextMeasurer.Measure(name, new TextOptions(font));
        return (fontRectangle.Width, fontRectangle.Height);
    }
}