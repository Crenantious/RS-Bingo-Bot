// <copyright file="GraphXAxisLabelsBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging.Graphs;

using SixLabors.Fonts;
using System.Collections.Generic;

public class GraphXAxisLabelsBuilder : GraphAxisLabelsBuilder
{
    private IEnumerable<int> labelXPositions;

    protected override HorizontalAlignment horizontalAlignment =>  HorizontalAlignment.Center;

    protected override VerticalAlignment verticalAlignment => VerticalAlignment.Top;

    public GraphXAxisLabelsBuilder(IEnumerable<string> labels, int axisWidth, int axisHeight, IEnumerable<int> labelXPositions)
        : base(labels, axisWidth, axisHeight)
    {
        this.labelXPositions = labelXPositions;
    }

    protected override Point GetLabelPosition(int index) =>
        new(labelXPositions.ElementAt(index), 0);

    protected override void IncreaseSizeFromNewLabel(FontRectangle labelRect)
    {
        if (labelRect.Height > Height) { Height = (int)labelRect.Height; }
    }
}