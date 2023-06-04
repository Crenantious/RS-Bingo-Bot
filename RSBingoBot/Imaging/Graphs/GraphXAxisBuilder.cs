// <copyright file="GraphAxisBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging.Graphs;

using SixLabors.ImageSharp;
using static GraphPreferences;

public class GraphXAxisBuilder : GraphAxisBuilder
{
    public GraphXAxisBuilder(int divisionCount) : base(divisionCount) { }

    protected override Point GetDivisionPosition(int index) =>
        new(GetDivisionOffset(index), AxisSpineThickness);

    protected override (int width, int height) GetDivisionSize() =>
        (GraphPreferences.DivisionThickness, GraphPreferences.DivisionLength);

    protected override (int width, int height) GetImageSize() =>
        (LongestSide, AxisSpineThickness + DivisionLength);

    protected override (int width, int height) GetSpinePosition() =>
        (0, 0);

    protected override (int width, int height) GetSpineSize() =>
        (Width, AxisSpineThickness);
}