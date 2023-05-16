// <copyright file="LeaderboardDiscord.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using DSharpPlus.Entities;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Models;
using RSBingo_Framework.Scoring;
using RSBingoBot.BingoCommands;
using SixLabors.ImageSharp;
using static LeaderboardImage;

public class LeaderboardDiscord
{
    private const string leaderboardImageFileName = "Leaderboard.png";
    private const string noMessageExceptionMessage = "The leaderboard message must first be created via a slash command.";

    private static readonly SemaphoreSlim semaphore = new(1, 1);
    private static DiscordMessage leaderboardMessage;

    public static async Task SetUp()
    {
        try { leaderboardMessage = await DataFactory.LeaderboardChannel.GetMessageAsync(DataFactory.LeaderboardMessageId); }
        catch { General.LoggingLog(new NullReferenceException(noMessageExceptionMessage), ""); }

        TeamScore.ScoreUpdatedEventAsync += UpdateLeaderboard;
        RSBingoBot.DiscordTeam.TeamCreatedEvent += AddTeam;
    }

    private static async Task AddTeam(RSBingoBot.DiscordTeam team)
    {
        // Recreating the board each time is very inefficient but the system does not need to be fast.
        await UpdateLeaderboard(() => Create().Save(leaderboardImageFileName));
    }

    private static async Task UpdateLeaderboard(TeamScore teamScore, Tile _)
    {
        // Recreating the board each time is very inefficient but the system does not need to be fast.
        await UpdateLeaderboard(() => Create().Save(leaderboardImageFileName));
    }

    private static async Task UpdateLeaderboard(Action createImage)
    {
        await semaphore.WaitAsync();
        FileStream? fs = null;

        try
        {
            createImage();
            fs = new(leaderboardImageFileName, FileMode.Open);
            await leaderboardMessage.ModifyAsync(new DiscordMessageBuilder()
                .AddFile(fs));
        }
        catch (Exception e)
        {
            // TODO: figure out what data to pass in.
            General.LoggingLog(e, "");
        }
        finally
        {
            fs?.Dispose();
            semaphore.Release();
        }
    }
}