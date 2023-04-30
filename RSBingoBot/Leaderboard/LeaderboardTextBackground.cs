// <copyright file="LeaderboardTextBackground.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static RSBingoBot.Leaderboard.LeaderboadPreferences;

internal class LeaderboardTextBackground
{
    private static int currentTextMaxHeight = MinimumBackgroundHeight;

    private int currentTextMaxWidth = MinimumBackgroundWidth;

    public static int Height => currentTextMaxHeight + TextPaddingHeight * 2 + TextBackgroundBorderThickness * 2;

    public int Width => currentTextMaxWidth + TextPaddingWidth * 2 + TextBackgroundBorderThickness * 2;
    public int XPosition { get; set; }
    public Image Image { get; private set; } = null!;
    public Point TextPosition { get; private set; }

    public LeaderboardTextBackground()
    {
        SetTextPosition();
        CreateImage();
    }

    /// <summary>
    /// If <paramref name="name"/> is of greater size than the current max size,
    /// then the max width and/or max height will be updated to match that of <paramref name="name"/>.
    /// </summary>
    /// <returns><see langword="true"/> if either the max width or max height was changed, <see langword="false"/> otherwise.</returns>
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

        if (isDirty) { SetTextPosition(); }

        return isDirty;
    }

    public void CreateImageIfDirty()
    {
        if ((Width > Image.Width || Height > Image.Height) is false) { return; }
        CreateImage();
    }

    private static (int width, int height) GetTextSize(string name)
    {
        FontRectangle fontRectangle = TextMeasurer.Measure(name, new TextOptions(LeaderboadPreferences.Font));
        int width = (int)MathF.Ceiling(fontRectangle.Width);
        int height = (int)MathF.Ceiling(fontRectangle.Height);
        return (width, height);
    }

    private void SetTextPosition() =>
        //TextPosition = new(TextPaddingWidth + TextBackgroundBorderThickness, TextPaddingHeight + TextBackgroundBorderThickness);
        TextPosition = new(Width/2, Height/2);

    private void CreateImage()
    {
        Image = new Image<Rgba32>(Width, Height, TextBackgroundColour);
        Console.WriteLine($"{Width}, {Height}");

        //TODO: test
        if (Width <= 0 || Height <= 0) { return; }

        Image.Mutate(x =>
            DrawRectangleExtensions.Draw(x,
                TextColour,
                TextBackgroundBorderThickness,
                new RectangleF(0, 0, Width - TextBackgroundBorderThickness, Height - TextBackgroundBorderThickness)));
    }
}