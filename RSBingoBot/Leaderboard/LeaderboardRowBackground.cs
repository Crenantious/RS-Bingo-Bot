// <copyright file="LeaderboardRowBackground.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using RSBingoBot.DTO;
using static RSBingoBot.Leaderboard.LeaderboadPreferences;

internal class LeaderboardRowBackground
{
    public Image Image { get; private set; }
    public LeaderboardCellBackground[] Cells { get; private set; }

    public LeaderboardRowBackground(LeaderboardRowDimensions dimensions)
    {
        Image = new Image<Rgba32>(dimensions.widths.Sum(), dimensions.height);
        Cells = new LeaderboardCellBackground[dimensions.widths.Count()];
        int xPosition = 0;

        for (int i = 0; i < dimensions.widths.Count(); i++)
        {
            int width = dimensions.widths.ElementAt(i);
            Cells[i] = new(width, dimensions.height, new(xPosition, 0));

            Image.Mutate(x => x.DrawImage(Cells[i].Image, Cells[i].Position, 1));
            xPosition += width - TextBackgroundBorderThickness;
        }
    }
}