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

    private Point mainTitlePos = new();
    private Point xAxisPos = new();
    private Point xAxisTitlePos = new();
    private Point xAxisLabelsPos = new();
    private Point yAxisPos = new();
    private Point yAxisTitlePos = new();
    private Point yAxisLabelsPos = new();

    private Image mainTitle;
    private Image xAxisTitle;
    private Image yAxisTitle;
    private GraphAxis xAxis;
    private GraphAxis yAxis;
    private Image xAxisLabels;
    private Image yAxisLabels;

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
        mainTitle = CreateMainTitle();
        xAxisTitle = CreateXAxisTitle();
        yAxisTitle = CreateYAxisTitle();
        xAxis = CreateXAxis();
        yAxis = CreateYAxis();
        xAxisLabels = CreateXAxisLables(xAxis);
        yAxisLabels = CreateYAxisLables(yAxis);

        SetXPositions();
        SetYPositions();

        Image image = new Image<Rgba32>(
            xAxisLabelsPos.X + xAxisLabels.Width + XPadding,
            xAxisTitlePos.Y + xAxisTitle.Height + YPadding);

        image.Mutate(x => x.DrawImage(mainTitle, mainTitlePos, 1));
        image.Mutate(x => x.DrawImage(xAxisTitle, xAxisTitlePos, 1));
        image.Mutate(x => x.DrawImage(yAxisTitle, yAxisTitlePos, 1));
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

    private void SetXPositions()
    {
        yAxisTitlePos.X = XPadding;
        yAxisLabelsPos.X = GetPosTitleSpacing(yAxisTitlePos.X, yAxisTitle.Width);
        yAxisPos.X = GetPosTextSpacing(yAxisLabelsPos.X, yAxisLabels.Width);
        xAxisPos.X = yAxisPos.X + yAxis.Image.Width - AxisSpineThickness;
        xAxisLabelsPos.X = xAxisPos.X;
        xAxisTitlePos.X = GetCentredImagePosFromWidth(xAxisPos.X, xAxis.Image, xAxisTitle);
        mainTitlePos.X = GetCentredImagePosFromWidth(xAxisPos.X, xAxis.Image, mainTitle);
    }

    private void SetYPositions()
    {
        mainTitlePos.Y = YPadding;
        yAxisPos.Y = GetPosTitleSpacing(mainTitlePos.Y, mainTitle.Height);
        yAxisLabelsPos.Y = yAxisPos.Y;
        yAxisTitlePos.Y = GetCentredImagePosFromHeight(yAxisPos.Y, yAxis.Image, yAxisTitle);
        xAxisPos.Y = yAxisPos.Y + yAxis.Image.Height - AxisSpineThickness;
        xAxisLabelsPos.Y = GetPosTextSpacing(xAxisPos.Y, xAxis.Image.Height);
        xAxisTitlePos.Y = GetPosTitleSpacing(xAxisLabelsPos.Y, xAxis.Image.Height);
    }

    private int GetPosTextSpacing(int pos, int size) =>
        pos + size + TextSpacing;

    private int GetPosTitleSpacing(int pos, int size) =>
        pos + size + TitleSpacing;

    private int GetCentredImagePosFromWidth(int referencePos, Image referenceImage, Image targetImage) =>
        referencePos + referenceImage.Width / 2 - targetImage.Width / 2;

    private int GetCentredImagePosFromHeight(int referencePos, Image referenceImage, Image targetImage) =>
        referencePos + referenceImage.Height / 2 - targetImage.Height / 2;

    private Image CreateMainTitle() =>
        new GraphMainTitleBuilder(Title).Build();

    private Image CreateXAxisTitle() =>
        new GraphXAxisTitleBuilder(Title).Build();

    private Image CreateYAxisTitle() =>
        new GraphYAxisTitleBuilder(Title).Build();

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