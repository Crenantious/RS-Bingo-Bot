// <copyright file="RequestDeleteTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingoBot.Discord_event_handlers;
using Serilog;

namespace RSBingoBot.BingoCommands;

/// <summary>
/// Request for deleting a team.
/// </summary>
public class RequestDeleteTeam : RequestBase
{
    private const string CouldNotFindTeamError = "Could not find team with name {0} for deletion";

    private readonly string teamName;
    private static SemaphoreSlim roleSemaphore = new SemaphoreSlim(1, 1);
    private static SemaphoreSlim channelSemaphore = new SemaphoreSlim(1, 1);

    public RequestDeleteTeam(InteractionContext ctx, IDataWorker dataWorker, string teamName)
        : base(ctx, dataWorker)
    {
        this.teamName = teamName;
    }

    public override async Task<RequestResponse> ProcessRequest()
    {
        ILogger<RequestDeleteTeam> logger = General.LoggingInstance<RequestDeleteTeam>();

        if (!await DeleteTeamRole(logger, ctx, teamName))
        {
            logger.LogInformation("Failed to delete role for {TeamName}", teamName);
        }

        if(!await DeleteTeamChannels(logger, ctx, teamName))
        {
            logger.LogInformation("Failed to delete one or more channels for {TeamName}", teamName);
        }

        if (DataWorker.Teams.GetByName(teamName) is not Team team)
        {
            logger.LogInformation("Failed to find team with name {TeamName}", teamName);
            return RequestFailed(CouldNotFindTeamError.FormatConst(teamName));
        }

        DataWorker.Teams.Remove(team);

        return RequestSuccess("Team deleted.");
    }

    private protected override bool ValidateSpesificsRequest() => true; // No additional validation required.


    private static async Task<bool> DeleteTeamRole(ILogger<RequestDeleteTeam> logger, InteractionContext ctx, string teamName)
    {
        await roleSemaphore.WaitAsync();
        try
        {
            if (GetTeamRole(ctx, teamName) is DiscordRole role)
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

    private static async Task<bool> DeleteTeamChannels(ILogger<RequestDeleteTeam> logger, InteractionContext ctx, string teamName)
    {
        await channelSemaphore.WaitAsync();
        try
        {
            foreach (var channelPair in ctx.Guild.Channels.Where(c => c.Value.Name.StartsWith(teamName)))
            {
                await channelPair.Value.DeleteAsync();
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
            channelSemaphore.Release();
        }

        return true;
    }
}
