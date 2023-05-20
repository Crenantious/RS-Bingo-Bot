// <copyright file="RequestDeleteTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

using RSBingoBot.Leaderboard;
using RSBingo_Framework.Models;
using RSBingo_Framework.Interfaces;
using Microsoft.Extensions.Logging;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

/// <summary>
/// Request for deleting a team.
/// </summary>
public class RequestDeleteTeam : RequestBase
{
    private const string CouldNotFindTeamError = "Could not find a team with name '{0}'.";
    private const string TeamSuccessfullyDeletedMessage = "Team deleted successfully.";

    private readonly string teamName;

    private static SemaphoreSlim roleSemaphore = new(1, 1);
    private static SemaphoreSlim channelSemaphore = new(1, 1);

    private ILogger<RequestDeleteTeam> logger;
    private Team team;

    public RequestDeleteTeam(InteractionContext ctx, IDataWorker dataWorker, string teamName) : base(ctx, dataWorker) =>
        this.teamName = teamName;

    public override async Task<bool> ProcessRequest()
    {
        logger = General.LoggingInstance<RequestDeleteTeam>();

        if (await DeleteTeamRole() is false)
        {
            logger.LogInformation("Failed to delete role for {TeamName}", teamName);
        }

        if (await DeleteTeamChannels() is false)
        {
            logger.LogInformation("Failed to delete one or more channels for {TeamName}", teamName);
        }

        // TODO: find out if this throws an exception
        DataWorker.Teams.Remove(team);

        // HACK: the dw is also saved when the request is finished. This will need to be handled properly.
        DataWorker.SaveChanges();
        RSBingoBot.DiscordTeam.TeamDeleted(team);
        await LeaderboardDiscord.Update(DataWorker);

        return ProcessSuccess(TeamSuccessfullyDeletedMessage);
    }

    private protected override bool ValidateSpecificRequest()
    {
        if (DataWorker.Teams.GetByName(teamName) is Team team)
        {
            this.team = team;
            return true;
        }

        SetResponseMessage(CouldNotFindTeamError.FormatConst(teamName));
        return false;
    }

    private async Task<bool> DeleteTeamRole()
    {
        await roleSemaphore.WaitAsync();
        try
        {
            if (GetTeamRole(team) is DiscordRole role)
            {
                await role.DeleteAsync();
            }
        }
        catch (Exception e)
        {
            // TOOO: JCH - look at: https://rehansaeed.com/logging-with-serilog-exceptions/
            logger.LogDebug(e.Message);
            return false;
        }
        finally
        {
            roleSemaphore.Release();
        }

        return true;
    }

    private async Task<bool> DeleteTeamChannels()
    {
        await channelSemaphore.WaitAsync();

        bool allChannelsDeleted = await TryDeleteChannel(team.CategoryChannelId);
        allChannelsDeleted = await TryDeleteChannel(team.BoardChannelId) && allChannelsDeleted;
        allChannelsDeleted = await TryDeleteChannel(team.GeneralChannelId) && allChannelsDeleted;
        allChannelsDeleted = await TryDeleteChannel(team.EvidencelChannelId) && allChannelsDeleted;
        allChannelsDeleted = await TryDeleteChannel(team.VoiceChannelId) && allChannelsDeleted;

        channelSemaphore.Release();
        return true;
    }

    private async Task<bool> TryDeleteChannel(ulong id)
    {
        try
        {
            await Ctx.Guild.GetChannel(id).DeleteAsync();
        }
        catch (Exception e)
        {
            // TOOO: JCH - look at: https://rehansaeed.com/logging-with-serilog-exceptions/
            logger.LogDebug(e.Message);
            return false;
        }
        return true;
    }
}