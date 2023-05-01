// <copyright file="LeaderboardTextDrawer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

internal static class LeaderboardTextDrawer
{
    /// <summary>
    /// Draws each value as text on the background. Positioned based on <see cref="LeaderboardRowBackground.Components"/>.<br/>
    /// <paramref name="values"/> must have the same Count as <see cref="LeaderboardRowBackground.Components"/>.
    /// </summary>
    /// <param name="image">The image to draw on.</param>
    /// <param name="values">The text to draw onto the image.</param>
    public static void DrawInfo(Image image, IEnumerable<string> values)
    {
        for (int i = 0; i < values.Count(); i++)
        {
            DrawText(image, values.ElementAt(i), GetTextPosition(LeaderboardRowBackground.Components[i]));
        }
    }

    private static Point GetTextPosition(LeaderboardRowBackgroundComponent background) =>
        new(background.Position.X + background.TextPosition.X, background.Position.Y + background.TextPosition.Y);

    private static void DrawText(Image image, string text, Point position)
    {
        TextOptions textOptions = new(LeaderboadPreferences.Font)
        {
            Origin = position,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        image.Mutate(x => x.DrawText(textOptions, text, LeaderboadPreferences.TextColour));
    }
}