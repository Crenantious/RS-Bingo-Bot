// <copyright file="GraphYAxisBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging.Graphs;

using SixLabors.ImageSharp;
using static GraphPreferences;

public class GraphYAxisBuilder : GraphAxisBuilder
{
    public GraphYAxisBuilder(int divisionCount) : base(divisionCount) { }

    protected override Point GetDivisionPosition(int index) =>
        new(0, GetDivisionOffset(index));

    protected override (int width, int height) GetDivisionSize() =>
        (GraphPreferences.DivisionLength, GraphPreferences.DivisionThickness);

    protected override (int width, int height) GetImageSize() =>
        (AxisSpineThickness + DivisionLength, LongestSide);

    protected override (int width, int height) GetSpinePosition() =>
        (DivisionLength, 0);

    protected override (int width, int height) GetSpineSize() =>
        (AxisSpineThickness, Height);
}