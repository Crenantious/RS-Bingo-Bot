// <copyright file="GraphYAxisLabelsBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging.Graphs;

using SixLabors.Fonts;
using System.Collections.Generic;

public class GraphYAxisLabelsBuilder : GraphAxisLabelsBuilder
{
    private IEnumerable<int> labelYPositions;

    protected override HorizontalAlignment horizontalAlignment => HorizontalAlignment.Left;

    protected override VerticalAlignment verticalAlignment => VerticalAlignment.Center;

    public GraphYAxisLabelsBuilder(IEnumerable<string> labels, int axisWidth, int axisHeight, IEnumerable<int> labelYPositions)
        : base(labels, axisWidth, axisHeight)
    {
        this.labelYPositions = labelYPositions;
    }

    protected override Point GetLabelPosition(int index) =>
        new(0, labelYPositions.ElementAt(index));

    protected override void IncreaseSizeFromNewLabel(FontRectangle labelRect)
    {
        if (labelRect.Width > Width) { Width = (int)labelRect.Width; }
    }
}