// <copyright file="LeaderboadPreferences.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using SixLabors.Fonts;
using SixLabors.ImageSharp;
using static RSBingo_Common.General;

// TODO: refactor in to appsettings.
internal static class LeaderboadPreferences
{
    public static Font Font { get; private set; } = null!;
    public static Color TextColour => Color.Black;
    public static Color TextBackgroundColour => Color.White;
    public static Color TextBackgroundBorderColour => Color.Black;
    public static int TextBackgroundBorderThickness => 1;
    public static int MinimumBackgroundWidth => 15;
    public static int MinimumBackgroundHeight => 10;
    public static int TextPaddingWidth => 10;
    public static int TextPaddingHeight => 5;

    private static string FontPath = Path.Combine(AppRootPath, "Resources/Fonts/Champagne & Limousines/Champagne & Limousines Bold.ttf");
    private static int FontSize = 18;
    private static FontStyle Style = FontStyle.Bold;

    static LeaderboadPreferences()
    {
        FontCollection collection = new();
        FontFamily family = collection.Add(FontPath);
        Font = family.CreateFont(FontSize, Style);
    }
}