// <copyright file="DeleteTeamRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.DTO;
using RSBingoBot.Exceptions;
using RSBingo_Framework.Models;
using Microsoft.Extensions.Logging;
using DSharpPlus.Entities;
using static RSBingoBot.MessageUtilities;

/// <summary>
/// Request for deleting a team.
/// </summary>
internal class DeleteTeamRequest : RequestHandlerBase
{
    private const string TeamDoesNotExistError = "A team with the name '{0}' does not exist.";
    private const string RoleDeletionError = "Failed to delete the role '{0}'.";
    private const string ChannelDeletionError = "Failed to delete the channel '{0}'.";
    private const string TeamSuccessfullyDeletedMessage = "Team '{0}' deleted successfully.";

    private readonly string teamName;

    private static SemaphoreSlim semaphore = new(1, 1);

    private Team team;

    public DeleteTeamRequest(DiscordInteraction interaction, string teamName) : base(semaphore) =>
        this.teamName = teamName;

    protected override async Task Process()
    {
        await DeleteRole();
        await DeleteChannels();

        // TODO: find out if this throws an exception
        DataWorker.Teams.Remove(team);
        RSBingoBot.DiscordTeam.TeamDeleted(team);
    }

    protected override bool Validate()
    {
        if (DataWorker.Teams.GetByName(teamName) is Team team)
        {
            this.team = team;
            return true;
        }

        return false;
    }

    private async Task DeleteRole()
    {
        throw new NotImplementedException();
    }

    private async Task DeleteChannels()
    {
        throw new NotImplementedException();
    }

    private async Task DeleteChannel(ulong id)
    {
        throw new NotImplementedException();
    }
}