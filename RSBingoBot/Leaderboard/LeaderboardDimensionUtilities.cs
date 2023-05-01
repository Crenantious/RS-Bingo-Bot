// <copyright file="LeaderboardDimensionUtilities.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using SixLabors.Fonts;
using RSBingoBot.DTO;
using RSBingo_Framework.Models;
using static RSBingoBot.Leaderboard.LeaderboadPreferences;

internal static class LeaderboardDimensionUtilities
{
    /// <summary>
    /// Based on the values to be drawn as text from <paramref name="teams"/>,
    /// the <see cref="LeaderboardRowDimensions.widths"/> are the maximum in each column and
    /// <see cref="LeaderboardRowDimensions.height"/> is the maximum overall.
    /// </summary>
    public static LeaderboardRowDimensions GetRowDimensions(IEnumerable<Team> teams)
    {
        float[] textWidths = Enumerable.Repeat((float)MinimumBackgroundWidth, 3).ToArray();
        float textHeight = MinimumBackgroundHeight;

        for (int i = 0; i < teams.Count(); i++)
        {
            (textWidths[0], textHeight) = GetMaxTextSize(teams.ElementAt(i).Name, textWidths[0], textHeight);
            (textWidths[1], textHeight) = GetMaxTextSize(teams.ElementAt(i).Score.ToString(), textWidths[1], textHeight);
            (textWidths[2], textHeight) = GetMaxTextSize(i.ToString(), textWidths[2], textHeight);
        }

        int xPadding = TextPaddingWidth * 2 + TextBackgroundBorderThickness * 2;
        int yPadding = TextPaddingHeight * 2 + TextBackgroundBorderThickness * 2;
        IEnumerable<int> widths = textWidths.Select(w => (int)MathF.Ceiling(w) + xPadding);
        textHeight += yPadding;

        return new(widths, (int)textHeight);
    }

    public static (int width, int height) GetBoardDimensions(LeaderboardRowDimensions rowDimensions, int rowCount) =>
        (rowDimensions.widths.Sum(), rowDimensions.height * rowCount - TextBackgroundBorderThickness * (rowCount - 1));

    private static (float width, float height) GetMaxTextSize(string value, float width, float height)
    {
        (float textWidth, float textHeight) = GetTextSize(value);
        return (MathF.Max(textWidth, width), MathF.Max(textHeight, height));
    }

    private static (float width, float height) GetTextSize(string name)
    {
        FontRectangle fontRectangle = TextMeasurer.Measure(name, new TextOptions(LeaderboadPreferences.Font));
        return (fontRectangle.Width, fontRectangle.Height);
    }
}