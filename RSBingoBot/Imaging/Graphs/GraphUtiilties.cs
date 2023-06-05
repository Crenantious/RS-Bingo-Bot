// <copyright file="GraphUtiilties.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging.Graphs;

using RSBingoBot.DTO;

public static class GraphUtiilties
{
            private const string LabelCountOutOfRange = "{0} must be greater than zero. Was {1}.";
    public static (IEnumerable<string> xAxisLabels, IEnumerable<string> yAxisLabels) GetAxisLabelsFromData(
        IEnumerable<LineGraphCategory> categories, int xAxisLabelCount, int yAxisLabelCount)
    {
        if(xAxisLabelCount < 1)
        {
            throw new ArgumentOutOfRangeException(LabelCountOutOfRange.FormatConst(nameof(xAxisLabelCount), xAxisLabelCount));
        }

        if (yAxisLabelCount < 1)
        {
            throw new ArgumentOutOfRangeException(LabelCountOutOfRange.FormatConst(nameof(xAxisLabelCount), yAxisLabelCount));
        }

        (float minX, float maxX, float minY, float maxY) = GetAxiesBoudaries(categories);

        string[] xLabels = new string[xAxisLabelCount];
        string[] yLabels = new string[yAxisLabelCount];
        float xStep = xAxisLabelCount == 1 ? 0 : (maxX - minX) / (xAxisLabelCount - 1);
        float yStep = yAxisLabelCount == 1 ? 0 : (maxY - minY) / (yAxisLabelCount - 1);

        for (int i = 0; i < xAxisLabelCount; i++)
        {
            xLabels[i] = (minX + (xStep * i)).ToString();
        }

        for (int i = 0; i < yAxisLabelCount; i++)
        {
            yLabels[i] = (minY + (yStep * i)).ToString();
        }

        return (xLabels, yLabels);
    }

    private static (float minX, float maxX, float minY, float maxY) GetAxiesBoudaries(IEnumerable<LineGraphCategory> categories)
    {
        float minX = 0;
        float maxX = 0;
        float minY = 0;
        float maxY = 0;

        foreach (LineGraphCategory category in categories)
        {
            foreach ((float x, float y) in category.dataPoints)
            {
                if (x < minX) { minX = x; }
                if (x > maxX) { maxX = x; }
                if (y < maxY) { minY = y; }
                if (y > maxY) { maxY = y; }
            }
        }
        return (minX, maxX, minY, maxY);
    }
}