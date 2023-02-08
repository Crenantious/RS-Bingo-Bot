// <copyright file="LeaderboardTeamBackground.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using SixLabors;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static RSBingoBot.Leaderboard.LeaderboadPreferences;

internal static class LeaderboardTeamBackground
{
    public static Image Image { get; private set; }
    public static LeaderboardTextBackground nameBackground { get; private set; } = new();
    public static LeaderboardTextBackground scoreBackground { get; private set; } = new();
    public static LeaderboardTextBackground rankBackground { get; private set; } = new();

    private static LeaderboardTextBackground[] textBackgrounds = { nameBackground, scoreBackground, rankBackground };

    static LeaderboardTeamBackground()
    {
        Image = CreateBackground();
    }

    public static void TryUpdateMaxSize(string name, string score, string rank)
    {
        if (nameBackground.TryUpdateMaxSize(name) || 
            scoreBackground.TryUpdateMaxSize(score) ||
            rankBackground.TryUpdateMaxSize(rank))
        {
            CreateBackground();
        }
    }

    //TODO: JR - make this more efficient by storing the indexes of each textBackground and only redrawing the ones
    // past the dirtied index.
    private static Image CreateBackground()
    {
        int[] componentXPositions = new int[textBackgrounds.Length];
        int height = MinimumBackgroundHeight;
        componentXPositions[0] = 0;

        for (int i = 1; i < textBackgrounds.Length - 1; i++)
        {
            componentXPositions[i] = componentXPositions[i - 1] + textBackgrounds[i].Image.Width;
            if (textBackgrounds[i].Image.Height > height) height = textBackgrounds[i].Image.Height;
        }

        Image background = new Image<Rgba32>(componentXPositions[^1], height);

        for (int i = 0; i < textBackgrounds.Length; i++)
        {
            background.Mutate(x => x.DrawImage(textBackgrounds[i].Image, new Point(componentXPositions[i], 0), 1));
        }

        return background;
    }
}