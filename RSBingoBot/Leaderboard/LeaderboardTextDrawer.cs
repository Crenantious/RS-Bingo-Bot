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
    /// Draws each value as text on the background. Positioned based on <paramref name="cells"/>.<br/>
    /// <paramref name="values"/> must have the same Count as <paramref name="cells"/>.
    /// </summary>
    public static void DrawInfo(Image image, IEnumerable<LeaderboardCellBackground> cells, IEnumerable<string> values)
    {
        for (int i = 0; i < values.Count(); i++)
        {
            DrawText(image, values.ElementAt(i), GetTextPosition(cells.ElementAt(i)));
        }
    }

    private static Point GetTextPosition(LeaderboardCellBackground background) =>
        new(background.Position.X + background.TextPosition.X, background.Position.Y + background.TextPosition.Y);

    public static void DrawText(Image image, string text, Point position)
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