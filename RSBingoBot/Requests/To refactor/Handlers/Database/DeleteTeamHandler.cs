// <copyright file="DeleteTeamHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using RSBingo_Framework.Models;

namespace RSBingoBot.Requests;

internal class DeleteTeamHandler : RequestHandler<DeleteTeamRequest>
{
    private const string TeamDoesNotExistError = "A team with the name '{0}' does not exist.";
    private const string RoleDeletionError = "Failed to delete the role '{0}'.";
    private const string ChannelDeletionError = "Failed to delete the channel '{0}'.";
    private const string TeamSuccessfullyDeletedMessage = "Team '{0}' deleted successfully.";

    private static readonly SemaphoreSlim semaphore = new(1, 1);

    public DeleteTeamHandler() : base(semaphore)
    {

    }

    protected override async Task Process(DeleteTeamRequest request, CancellationToken cancellationToken)
    {
        // TODO: JE - implement.
        await DeleteRole();
        await DeleteChannels();

        Team team = DataWorker.Teams.GetByName(request.TeamName)!;
        DataWorker.Teams.Remove(team);
        RSBingoBot.DiscordTeam.TeamDeleted(team);
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