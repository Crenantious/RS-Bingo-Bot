// <copyright file="LineGraphBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging.Graphs;

using RSBingoBot.DTO;
using static GraphPreferences;

public class LineGraphBuilder : GraphBuilder
{
    private IEnumerable<LineGraphCategory> categories;
    private string legendTitle;

    private Point yAxisLabelsPos = new();
    private Point yAxisPos = new();
    private Point xAxisPos = new();
    private Point xAxisLabelsPos = new();

    public LineGraphBuilder(string title, GraphAxisInfo xAxisInfo, GraphAxisInfo yAxisInfo,
        IEnumerable<LineGraphCategory> categories, string legendTitle) : base(title, xAxisInfo, yAxisInfo)
    {
        this.categories = categories;
        this.legendTitle = legendTitle;
    }

    public Image Build()
    {
        GraphAxis xAxis = CreateXAxis();
        GraphAxis yAxis = CreateYAxis();
        Image xAxisLabels = CreateXAxisLables(xAxis);
        Image yAxisLabels = CreateYAxisLables(yAxis);
        xAxis.Image.Save(Path.Combine(Paths.ResourcesTestOutputFolder, "xAxis.png"));
        yAxis.Image.Save(Path.Combine(Paths.ResourcesTestOutputFolder, "yAxis.png"));
        xAxisLabels.Save(Path.Combine(Paths.ResourcesTestOutputFolder, "xAxisLabels.png"));
        yAxisLabels.Save(Path.Combine(Paths.ResourcesTestOutputFolder, "yAxisLabels.png"));

        SetXPositions(yAxis, yAxisLabels);
        SetYPositions(yAxis, xAxis);

        Image image = new Image<Rgba32>(
            xAxisLabelsPos.X + xAxisLabels.Width + XPadding,
            xAxisLabelsPos.Y + xAxisLabels.Height + YPadding);

        image.Mutate(x => x.DrawImage(xAxis.Image, xAxisPos, 1));
        image.Mutate(x => x.DrawImage(yAxis.Image, yAxisPos, 1));
        image.Mutate(x => x.DrawImage(xAxisLabels, xAxisLabelsPos, 1));
        image.Mutate(x => x.DrawImage(yAxisLabels, yAxisLabelsPos, 1));

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