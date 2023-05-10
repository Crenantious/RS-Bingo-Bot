// <copyright file="LeaderboardDiscord.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using DSharpPlus.Entities;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Scoring;
using SixLabors.ImageSharp;
using static LeaderboardImage;

public class LeaderboardDiscord
{
    private const string leaderboardImageFileName = "Leaderboard.png";

    private static SemaphoreSlim semaphore = new(1, 1);

    public static async Task SetUp()
    {
        //TODO: JR - remove startup posing when finished with tests
        await PostLeaderboard(Create());
        TeamScore.ScoreUpdatedEventAsync += UpdateLeaderboard;
    }

    private static async Task UpdateLeaderboard(TeamScore teamScore)
    {
        try
        {
            await semaphore.WaitAsync();

            // Recreating the board each time is very inefficient but the system does not need to be fast.
            await PostLeaderboard(Create());
        }
        finally
        {
            semaphore.Release();
        }
    }

    private static async Task PostLeaderboard(Image leaderboard)
    {
        DiscordMessage imageMessage;
        leaderboard.SaveAsPng(leaderboardImageFileName);

        FileStream fs = new(leaderboardImageFileName, FileMode.Open);

        imageMessage = await DataFactory.LeaderboardChannel.SendMessageAsync(new DiscordMessageBuilder()
            .AddFile(fs));
    }
}