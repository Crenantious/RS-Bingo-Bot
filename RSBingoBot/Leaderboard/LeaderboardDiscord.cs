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

    public static void SetUp()
    {
        Create();
        TeamScore.ScoreUpdatedEventAsync += UpdateLeaderboard;
    }

    private static async Task UpdateLeaderboard(TeamScore teamScore)
    {
        // Recreating the board each time is very inefficient but the system does not need to be fast.
        await PostLeaderboard(Create());
    }

    private static async Task PostLeaderboard(Image leaderboard)
    {
        DiscordMessage imageMessage;
        leaderboard.SaveAsPng(leaderboardImageFileName);

        using (var fs = new FileStream(leaderboardImageFileName, FileMode.Open, FileAccess.Read))

        imageMessage = await DataFactory.LeaderboardChannel.SendMessageAsync(new DiscordMessageBuilder()
            .WithFile("Leaderboard", fs));
    }
}