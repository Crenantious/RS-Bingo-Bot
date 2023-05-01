// <copyright file="LeaderboardRowBackground.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using RSBingoBot.DTO;
using static RSBingoBot.Leaderboard.LeaderboadPreferences;

internal static class LeaderboardRowBackground
{
    public static Image Image { get; private set; }
    public static LeaderboardRowBackgroundComponent[] Components { get; private set; }

    public static Image Create(LeaderboardRowDimensions dimensions)
    {
        Image = new Image<Rgba32>(dimensions.widths.Sum(), dimensions.height);
        Components = new LeaderboardRowBackgroundComponent[dimensions.widths.Count()];
        int xPosition = 0;

        for (int i = 0; i < dimensions.widths.Count(); i++)
        {
            int width = dimensions.widths.ElementAt(i);
            Components[i] = new(width, dimensions.height, new(xPosition, 0));

            Image.Mutate(x => x.DrawImage(Components[i].Image, Components[i].Position, 1));
            xPosition += width - TextBackgroundBorderThickness;
        }

        return Image;
    }
}