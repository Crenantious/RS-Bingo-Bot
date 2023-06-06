// <copyright file="LineGraphDataPlotBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging.Graphs;

using SixLabors.ImageSharp.Drawing.Processing;
using static GraphPreferences;

internal class LineGraphDataPlotBuilder<TXData, TYData>
{
    private IEnumerable<(TXData x, TYData y)> Data;
    private Func<TXData, TXData, TXData, float> getXMinMaxRatio;
    private Func<TYData, TYData, TYData, float> getYMinMaxRatio;
    private int width;
    private int height;
    private int xMinPos;
    private int xMaxPos;
    private int yMinPos;
    private int yMaxPos;
    private Color color;

    private TXData xMin;
    private TXData xMax;
    private TYData yMin;
    private TYData yMax;
    private float xPosRange;
    private float yPosRange;

    private Image image = null!;

    // Assumes data is ordered
    public LineGraphDataPlotBuilder(IEnumerable<(TXData x, TYData y)> Data,
        Func<TXData, TXData, TXData, float> getXMinMaxRatio,
        Func<TYData, TYData, TYData, float> getYMinMaxRatio,
        TXData xMin, TXData xMax, TYData yMin, TYData yMax,
        int width, int height, int xMinPos, int xMaxPos, int yMinPos, int yMaxPos,
        Color color)
    {
        this.Data = Data;
        this.getXMinMaxRatio = getXMinMaxRatio;
        this.getYMinMaxRatio = getYMinMaxRatio;
        this.width = width;
        this.height = height;
        this.xMinPos = xMinPos;
        this.xMaxPos = xMaxPos;
        this.yMinPos = yMinPos;
        this.yMaxPos = yMaxPos;
        this.color = color;
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;
        this.yMax = yMax;

        xPosRange = xMaxPos - xMinPos;
        yPosRange = yMaxPos - yMinPos;
    }

    public Image Build()
    {
        image = new Image<Rgba32>(width, height);
        PlotData();
        return image;
    }

    private void PlotData()
    {
        PointF[] dataPoints = new PointF[Data.Count()];

        for (int i = 0; i < Data.Count(); i++)
        {
            var x = Data.ElementAt(i).x;
            var y = Data.ElementAt(i).y;
            float xRatio = getXMinMaxRatio.Invoke(x, xMin, xMax);

            // y pos is inverted compared to the axis labels.
            float yRatio = 1 - getYMinMaxRatio.Invoke(y, yMin, yMax);
            PointF point = new(xMinPos + xPosRange * xRatio, yMinPos + yPosRange * yRatio);
            RectangleF rect = new(point.X - DataMarkerWidth / 2,
                                  point.Y - DataMarkerHeight / 2,
                                  DataMarkerWidth,
                                  DataMarkerHeight);

            image.Mutate(x => FillRectangleExtensions.Fill(x, color, rect));
            dataPoints[i] = point;
        }

        image.Mutate(x => x.DrawLines(color, DataLineThickness, dataPoints));
    }
}