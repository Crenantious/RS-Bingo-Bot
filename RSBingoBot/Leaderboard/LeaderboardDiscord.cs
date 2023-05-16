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
    }

    public static async Task Update()
    {
        FileStream? fs = null;

        try
        {
            await semaphore.WaitAsync();

            Create().Save(leaderboardImageFileName);
            fs = new(leaderboardImageFileName, FileMode.Open);

            // TODO: this gets posted as a 0KB file if the bot is rate limited. Keep trying to send or find out when the rate limit is over.
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