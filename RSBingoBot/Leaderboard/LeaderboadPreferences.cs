// <copyright file="LeaderboadPreferences.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using SixLabors.Fonts;
using SixLabors.ImageSharp;
using static RSBingo_Common.General;
namespace RSBingoBot.Leaderboard;

internal record LeaderboadPreferences
{
    public static Font Font { get; private set; } = null!;
    public static Color TextColour { get; private set; } = Color.Black;
    public static Color TextBackgroundBorderColour { get; private set; } = Color.Black;
    public static float TextBackgroundBorderThickness { get; private set; } = 1;
    public static int MinimumBackgroundWidth => 15;
    public static int MinimumBackgroundHeight => 10;
    public static int TextPaddingWidth => 10;
    public static int TextPaddingHeight => 5;

    private static string FontPath => Path.Combine(AppRootPath, "Fonts\\Champagne & Limousines\\Champagne & Limousines Bold.ttf");
    private static int FontSize => 18;
    private static FontStyle Style => FontStyle.Bold;

    static LeaderboadPreferences() =>
        CreateFont();

    private static void CreateFont()
    {
        FontCollection collection = new();
        FontFamily family = collection.Add(FontPath);
        Font = family.CreateFont(FontSize, Style);
    }
}