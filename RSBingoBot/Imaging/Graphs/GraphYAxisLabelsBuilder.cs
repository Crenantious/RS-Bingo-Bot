// <copyright file="GraphYAxisLabelsBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging.Graphs;

using SixLabors.Fonts;
using System.Collections.Generic;

public class GraphYAxisLabelsBuilder : GraphAxisLabelsBuilder
{
    private IEnumerable<int> labelYPositions;
    private int labelCount;

    protected override HorizontalAlignment horizontalAlignment => HorizontalAlignment.Left;

    protected override VerticalAlignment verticalAlignment => VerticalAlignment.Center;

    public GraphYAxisLabelsBuilder(IEnumerable<string> labels, int axisWidth, int axisHeight,
        IEnumerable<int> labelYPositions, bool ignoreEndpoints = true)
        : base(labels, axisWidth, axisHeight, ignoreEndpoints)
    {
        this.labelYPositions = labelYPositions;
        labelCount = labelYPositions.Count();
    }

    protected override Point GetLabelPosition(int index) =>
        // Gets drawn top down, but the values need to be ascending from bottom up.
        new(0, labelYPositions.ElementAt(labelCount - 1 - index));

    protected override void IncreaseSizeFromNewLabel(FontRectangle labelRect)
    {
        if (labelRect.Width > Width) { Width = (int)labelRect.Width; }
    }
}