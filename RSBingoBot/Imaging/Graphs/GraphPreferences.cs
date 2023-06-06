// <copyright file="GraphPreferences.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging.Graphs;

using SixLabors.Fonts;

internal class GraphPreferences
{
    /// <summary>
    /// The padding to add to each horizontal side of the graph.
    /// </summary>
    /// 
    public const int XPadding = 20;
    /// <summary>
    /// The padding to add to each vertical side of the graph.
    /// </summary>
    public const int YPadding = 20;

    public const int DivisionSpacing = 100;
    public const int DivisionThickness = 5;
    public const int DivisionLength = 12;

    /// <summary>
    /// The size of the axis spine. This is the bar that spans the axis and divisions protrude from.
    /// </summary>
    public const int AxisSpineThickness = 5;

    public const int TextSpacing = 5;
    public const int TitleSpacing = 15;

    public const int DataMarkerWidth = 10;
    public const int DataMarkerHeight = 10;
    public const int DataLineThickness = 5;

    public const int LegendBorderThickness = 3;

    public static Font TitleFont => Fonts.CreateChampagneAndLimousines(24);
    public static Font TextFont => Fonts.CreateChampagneAndLimousines(18);
    public static Color TextColour => Color.Black;
    public static Color AxisColour => Color.Black;
    public static Color LegendBorderColour => Color.Black;
}