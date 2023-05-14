// <copyright file="LeaderboardDiscord.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using DSharpPlus.Entities;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Models;
using RSBingo_Framework.Scoring;
using SixLabors.ImageSharp;
using static LeaderboardImage;

public class LeaderboardDiscord
{
    private const string leaderboardImageFileName = "Leaderboard.png";

    private static SemaphoreSlim semaphore = new(1, 1);

    public static async Task SetUp() =>
        TeamScore.ScoreUpdatedEventAsync += UpdateLeaderboard;

    private static async Task UpdateLeaderboard(TeamScore teamScore, Tile _)
    {
        await semaphore.WaitAsync();

        // Recreating the board each time is very inefficient but the system does not need to be fast.
        Create().Save(leaderboardImageFileName);

        // The file stream is created here so it can be disposed of in the finally block.
        FileStream fs = new(leaderboardImageFileName, FileMode.Open);
        
        try
        {
            await DataFactory.LeaderboardChannel.SendMessageAsync(new DiscordMessageBuilder()
            .AddFile(fs));
        }
        finally
        {
            fs.Dispose();
            semaphore.Release();
        }
    }
}