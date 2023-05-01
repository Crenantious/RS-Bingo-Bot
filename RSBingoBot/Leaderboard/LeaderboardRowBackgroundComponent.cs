// <copyright file="LeaderboardRowBackgroundComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static RSBingoBot.Leaderboard.LeaderboadPreferences;

internal class LeaderboardRowBackgroundComponent
{
    public Image Image { get; private set; } = null!;
    public Point TextPosition { get; private set; }

    /// <summary>
    /// The position on the <see cref="LeaderboardRowBackground.Image"/>.
    /// </summary>
    public Point Position { get; private set; }

    public LeaderboardRowBackgroundComponent(int width, int height, Point position)
    {
        Position = position;
        CreateImage(width, height);
        SetTextPosition();
    }

    private void SetTextPosition() =>
        TextPosition = new(Image.Width / 2, Image.Height / 2);

    private void CreateImage(int width, int height)
    {
        Image = new Image<Rgba32>(width, height, TextBackgroundColour);

        //TODO: test
        if (width <= 0 || height <= 0) { return; }

        Image.Mutate(x =>
            DrawRectangleExtensions.Draw(x,
                TextColour,
                TextBackgroundBorderThickness,
                new RectangleF(0, 0, width - TextBackgroundBorderThickness, height - TextBackgroundBorderThickness)));
    }
}