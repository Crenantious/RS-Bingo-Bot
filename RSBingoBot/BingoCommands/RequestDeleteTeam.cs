// <copyright file="RequestDeleteTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

using RSBingoBot.DTO;
using RSBingoBot.Exceptions;
using RSBingo_Framework.Models;
using Microsoft.Extensions.Logging;
using DSharpPlus.Entities;
using static RSBingoBot.MessageUtilities;

/// <summary>
/// Request for deleting a team.
/// </summary>
internal class RequestDeleteTeam : RequestBase
{
    private const string TeamDoesNotExistError = "A team with the name '{0}' does not exist.";
    private const string RoleDeletionError = "Failed to delete the role '{0}'.";
    private const string ChannelDeletionError = "Failed to delete the channel '{0}'.";
    private const string TeamSuccessfullyDeletedMessage = "Team '{0}' deleted successfully.";

    private readonly string teamName;

    private static SemaphoreSlim semaphore = new(1, 1);

    private Team team;

    public RequestDeleteTeam(DiscordInteraction interaction, string teamName) : base(semaphore) =>
        this.teamName = teamName;

    protected override async Task<RequestResult<string>> Process()
    {
        List<string> errors = new(2);
        RequestResult<string> roleResult = await DeleteRole();
        RequestResult<string> channelsResult = await DeleteChannels();

        if (roleResult.IsFaulted)
        {
            errors.Concat(roleResult.Errors);
            Logger.LogInformation(roleResult.exception, GetCompiledMessage(roleResult.Errors));
        }

        if (channelsResult.IsFaulted)
        {
            errors.Concat(roleResult.Errors);
            Logger.LogInformation(roleResult.exception, GetCompiledMessage(roleResult.Errors));
            Logger.LogInformation(ChannelDeletionError, teamName);
        }

        // TODO: find out if this throws an exception
        DataWorker.Teams.Remove(team);
        RSBingoBot.DiscordTeam.TeamDeleted(team);

        return new(TeamSuccessfullyDeletedMessage.FormatConst(teamName));
    }

    protected override RequestResult Validate()
    {
        if (DataWorker.Teams.GetByName(teamName) is Team team)
        {
            this.team = team;
            return new();
        }

        return new(new RequestException(TeamDoesNotExistError));
    }

    private async Task<RequestResult<string>> DeleteRole()
    {
        throw new NotImplementedException();
    }

    private async Task<RequestResult<string>> DeleteChannels()
    {
        throw new NotImplementedException();
    }

    private async Task<RequestResult<string>> TryDeleteChannel(ulong id)
    {
        throw new NotImplementedException();
    }
}