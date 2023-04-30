// <copyright file="LeaderboardTeamBackground.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

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

    public static bool TryUpdateMaxSize(string name, string score, string rank)
    {
        bool isDirty = TryUpdateTextBackgroundSizes(name, score, rank);

        if (isDirty is false) { return false; }

        foreach (LeaderboardTextBackground background in textBackgrounds)
        {
            background.CreateImageIfDirty();
        }

        Image = CreateBackground();
        return true;
    }

    private static bool TryUpdateTextBackgroundSizes(params string[] values)
    {
        bool isDirty = false;
        for (int i = 0; i < textBackgrounds.Length; i++)
        {
            if (textBackgrounds[i].TryUpdateMaxSize(values[i])) { isDirty = true; }
        }
        return isDirty;
    }

    //TODO: JR - make this more efficient by storing the indexes of each textBackground and only redrawing the ones
    // past the dirtied index.
    private static Image CreateBackground()
    {
        SetBackgroundsXPositions();
        int width = textBackgrounds[^1].XPosition + textBackgrounds[^1].Image.Width;

        // Size must be increased by one to fit the border.
        Image background = new Image<Rgba32>(width, LeaderboardTextBackground.Height);

        for (int i = 0; i < textBackgrounds.Length; i++)
        {
            background.Mutate(x => x.DrawImage(textBackgrounds[i].Image, new Point(textBackgrounds[i].XPosition, 0), 1));
        }

        return background;
    }

    private static void SetBackgroundsXPositions()
    {
        for (int i = 1; i < textBackgrounds.Length; i++)
        {
            // Move to the left by one so we don't get double borders where the backgrounds meet;
            textBackgrounds[i].XPosition = textBackgrounds[i - 1].XPosition + textBackgrounds[i - 1].Image.Width - 1;
        }
    }

    record BackgroundInfo(int[] XPositions, int Width);
}