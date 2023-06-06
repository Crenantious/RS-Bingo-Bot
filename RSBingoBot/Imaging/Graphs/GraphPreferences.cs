// <copyright file="GraphPreferences.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging.Graphs;

using SixLabors.Fonts;

internal class GraphPreferences
{
    public const int XPadding = 20;
    public const int YPadding = 20;

    public const int DivisionSpacing = 100;
    public const int DivisionThickness = 5;
    public const int DivisionLength = 12;
    public const int AxisSpineThickness = 5;

    public const int TextSpacing = 5;
    public const int TitleSpacing = 15;
    public const int ScaleWidth = 50;

    public static Font LabelFont => Fonts.CreateChampagneAndLimousines(18);
    public static Font TitleFont => Fonts.CreateChampagneAndLimousines(24);
    public static Color TextColour => Color.Black;
    public static Color AxisColour => Color.Black;

    public static int DataMarkerWidth = 10;
    public static int DataMarkerHeight = 10;
    public static int DataLineThickness = 5;
}