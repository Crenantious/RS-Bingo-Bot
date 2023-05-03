// <copyright file="GridImageUtilities.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using RSBingoBot.Leaderboard;

namespace RSBingoBot.Imaging;

using RSBingoBot.DTO;
using SixLabors.Fonts;

internal static class GridImageUtilities
{
    public static GridImageDimensions GetGridImageDimensions(string[,] cellValues, int xPadding = 0, int yPadding = 0)
    {
        float[] colmnWidths = new float[cellValues.GetLength(0)];
        float[] rowHeights = new float[cellValues.GetLength(1)];

        for (int i = 0; i < colmnWidths.Length; i++)
        {
            for (int j = 0; j < rowHeights.Length; j++)
            {
                (float width, float height) = GetTextSize(cellValues[i, j]);
                colmnWidths[i] = MathF.Max(colmnWidths[i], width);
                rowHeights[j] = MathF.Max(rowHeights[j], height);
            }
        }

        IEnumerable<int> widths = colmnWidths.Select(w => (int)MathF.Ceiling(w) + xPadding);
        IEnumerable<int> heights = rowHeights.Select(h => (int)MathF.Ceiling(h) + yPadding);

        return new(widths, heights);
    }

    private static (float width, float height) GetTextSize(string name)
    {
        FontRectangle fontRectangle = TextMeasurer.Measure(name, new TextOptions(LeaderboadPreferences.Font));
        return (fontRectangle.Width, fontRectangle.Height);
    }
}