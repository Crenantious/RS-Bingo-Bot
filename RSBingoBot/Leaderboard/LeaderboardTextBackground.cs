// <copyright file="LeaderboardTextBackground.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace RSBingoBot.Leaderboard;

internal class LeaderboardTextBackground
{
    public Image Image { get; private set; }
    public Point Centre { get; private set; }

    private int currentTextMaxWidth = LeaderboadPreferences.MinimumBackgroundWidth;
    private int currentTextMaxHeight = LeaderboadPreferences.MinimumBackgroundHeight;

    public LeaderboardTextBackground() =>
        Image = CreateBackground();

    public bool TryUpdateMaxSize(string name)
    {
        (int textWidth, int textHeight) = GetTextSize(name);
        bool isDirty = false;

        if (textWidth > currentTextMaxWidth)
        {
            currentTextMaxWidth = textWidth;
            isDirty = true;
        }

        if (textHeight > currentTextMaxHeight)
        {
            currentTextMaxHeight = textHeight;
            isDirty = true;
        }

        if (isDirty)
        {
            Image box = CreateBackground();
        }

        return isDirty;
    }

    private static (int width, int height) GetTextSize(string name)
    {
        FontRectangle fontRectangle = TextMeasurer.Measure(name, new TextOptions(LeaderboadPreferences.Font));
        int width = (int)MathF.Ceiling(fontRectangle.Width);
        int height = (int)MathF.Ceiling(fontRectangle.Height);
        return (width, height);
    }

    private Image CreateBackground()
    {
        Image image = new Image<Rgba32>(currentTextMaxWidth, currentTextMaxHeight);
        image.Mutate(x =>
            DrawRectangleExtensions.Draw(x, LeaderboadPreferences.TextColour,
            LeaderboadPreferences.TextBackgroundBorderThickness, new RectangleF(0, 0, image.Width, image.Height)));
        return image;
    }
}