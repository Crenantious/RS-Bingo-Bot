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
    /// Gets the dimensions based on the maximum text width of cell values in each column and maximum height in each row. Then adds the padding.
    /// </summary>
    /// <param name="cellValues">The text values to go in each cell. Dimension 0 is the columns, and dimension 1 is the rows.</param>
    /// <param name="font">The font with which to measure the text's pixel size.</param>
    /// <param name="xPadding">The amount of pixels to add to the left and right of each cell; the cell width is increased by twice this value.</param>
    /// <param name="yPadding">The amount of pixels to add to the top and bottom of each cell; the cell height is increased by twice this value.</param>
    /// <returns>The constructed dimensions.</returns>
    public static GridImageDimensions GetGridImageDimensions(string[,] cellValues, Font font, int xPadding = 0, int yPadding = 0)
    {
        float[] columnWidths = new float[cellValues.GetLength(0)];
        float[] rowHeights = new float[cellValues.GetLength(1)];

        for (int i = 0; i < columnWidths.Length; i++)
        {
            for (int j = 0; j < rowHeights.Length; j++)
            {
                (float width, float height) = GetTextSize(cellValues[i, j], font);
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