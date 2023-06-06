// <copyright file="LeaderboadPreferences.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using RSBingoBot.Imaging;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

// TODO: refactor in to appsettings.
internal static class LeaderboadPreferences
{
    public static Font Font => Fonts.CreateChampagneAndLimousines(18);
    public static Color TextColour => Color.Black;
    public static Color TextBackgroundColour => Color.White;
    public static Color TextBackgroundBorderColour => Color.Black;
    public static int TextBackgroundBorderThickness => 1;
    public static int MinimumBackgroundWidth => 15;
    public static int MinimumBackgroundHeight => 10;
    public static int TextPaddingWidth => 10;
    public static int TextPaddingHeight => 5;
}