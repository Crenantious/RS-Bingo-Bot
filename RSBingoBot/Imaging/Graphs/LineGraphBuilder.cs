// <copyright file="LineGraphBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging.Graphs;

using RSBingoBot.DTO;
using static GraphPreferences;

public class LineGraphBuilder<TXData, TYData> : GraphBuilder
{
    private IEnumerable<LineGraphCategory<TXData, TYData>> categories;
    private TXData xMinValue;
    private TXData xMaxValue;
    private TYData yMinValue;
    private TYData yMaxValue;
    private Func<TXData, TXData, TXData, float> getXMinMaxRatio;
    private Func<TYData, TYData, TYData, float> getYMinMaxRatio;
    private string legendTitle;

    private Point yAxisLabelsPos = new();
    private Point yAxisPos = new();
    private Point xAxisPos = new();
    private Point xAxisLabelsPos = new();


    public LineGraphBuilder(string title, GraphAxisInfo xAxisInfo, GraphAxisInfo yAxisInfo,
        IEnumerable<LineGraphCategory<TXData, TYData>> categories,
        TXData xMinValue, TXData xMaxValue, TYData yMinValue, TYData yMaxValue,
        Func<TXData, TXData, TXData, float> getXMinMaxRatio,
        Func<TYData, TYData, TYData, float> getYMinMaxRatio,
        string legendTitle) : base(title, xAxisInfo, yAxisInfo)
    {
        this.categories = categories;
        this.xMinValue = xMinValue;
        this.xMaxValue = xMaxValue;
        this.yMinValue = yMinValue;
        this.yMaxValue = yMaxValue;
        this.getXMinMaxRatio = getXMinMaxRatio;
        this.getYMinMaxRatio = getYMinMaxRatio;
        this.legendTitle = legendTitle;
    }

    public Image Build()
    {
        GraphAxis xAxis = CreateXAxis();
        GraphAxis yAxis = CreateYAxis();
        Image xAxisLabels = CreateXAxisLables(xAxis);
        Image yAxisLabels = CreateYAxisLables(yAxis);

        SetXPositions(yAxis, yAxisLabels);
        SetYPositions(yAxis, xAxis);

        Image image = new Image<Rgba32>(
            xAxisLabelsPos.X + xAxisLabels.Width + XPadding,
            xAxisLabelsPos.Y + xAxisLabels.Height + YPadding);

        image.Mutate(x => x.DrawImage(xAxis.Image, xAxisPos, 1));
        image.Mutate(x => x.DrawImage(yAxis.Image, yAxisPos, 1));
        image.Mutate(x => x.DrawImage(xAxisLabels, xAxisLabelsPos, 1));
        image.Mutate(x => x.DrawImage(yAxisLabels, yAxisLabelsPos, 1));

        foreach (LineGraphCategory<TXData, TYData> category in categories)
        {
            LineGraphDataPlotBuilder<TXData, TYData> plotBuilder = new(category.Data, getXMinMaxRatio, getYMinMaxRatio,
                xMinValue, xMaxValue, yMinValue, yMaxValue,
                xAxis.Image.Width, yAxis.Image.Height,
                xAxis.DivisionPositions[0].X, xAxis.DivisionPositions[^1].X,
                yAxis.DivisionPositions[0].Y, yAxis.DivisionPositions[^1].Y,
                category.LegendColour);
            Image plotImage = plotBuilder.Build();
            image.Mutate(x => x.DrawImage(plotImage, new Point(xAxisPos.X - AxisSpineThickness, yAxisPos.Y), 1));
        }

        return image;
    }

    private void SetXPositions(GraphAxis yAxis, Image yAxisLabels)
    {
        yAxisLabelsPos.X = XPadding;
        yAxisPos.X = yAxisLabelsPos.X + yAxisLabels.Width + TextSpacing;
        xAxisPos.X = yAxisPos.X + yAxis.Image.Width - AxisSpineThickness;
        xAxisLabelsPos.X = xAxisPos.X;
    }

    private void SetYPositions(GraphAxis yAxis, GraphAxis xAxis)
    {
        yAxisLabelsPos.Y = YPadding;
        yAxisPos.Y = yAxisLabelsPos.Y;
        xAxisPos.Y = yAxisPos.Y + yAxis.Image.Height - AxisSpineThickness;
        xAxisLabelsPos.Y = xAxisPos.Y + xAxis.Image.Height + TextSpacing;
    }

    private GraphAxis CreateXAxis() =>
        new GraphXAxisBuilder(XAxisInfo.DivisionCount).Build();

    private GraphAxis CreateYAxis() =>
        new GraphYAxisBuilder(YAxisInfo.DivisionCount).Build();

    private Image CreateXAxisLables(GraphAxis axis) =>
        new GraphXAxisLabelsBuilder(XAxisInfo.CategoryLabels,
            axis.Image.Width,
            axis.Image.Height,
            axis.DivisionPositions.Select(p => p.X))
        .Build();

    private Image CreateYAxisLables(GraphAxis axis) =>
        new GraphYAxisLabelsBuilder(YAxisInfo.CategoryLabels,
            axis.Image.Width,
            axis.Image.Height,
            axis.DivisionPositions.Select(p => p.Y))
        .Build();
}