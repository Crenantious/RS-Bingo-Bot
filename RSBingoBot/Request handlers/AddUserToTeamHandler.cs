// <copyright file="AddUserToTeamRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using RSBingo_Framework.DAL;

internal class AddUserToTeamHandler : RequestHandler<AddUserToTeamRequest>
{
    private const string UserSuccessfullyAddedMessage = "The user '{0}' has been added to the team successfully.";
    private const string TeamRoleDoesNotExistWarning = "The team's role does not exist.";

    // TODO: JR - consider having one semaphore for all database write requests to avoid conflicts.
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    public AddUserToTeamHandler() : base(semaphore)
    {

    }

    protected override async Task Process(AddUserToTeamRequest request, CancellationToken cancellationToken)
    {
        ulong userId = request.User.Id;
        string teamName = request.TeamName;

        DataWorker.Users.Create(userId, teamName);
        AddSuccess(UserSuccessfullyAddedMessage);

        DiscordRole? role = RequestsUtilities.GetTeamRole(DataWorker, teamName);
        if (role is null)
        {
            AddWarning(TeamRoleDoesNotExistWarning);
            return;
        }

        DiscordMember member = await DataFactory.Guild.GetMemberAsync(userId);
        await member.GrantRoleAsync(role);
    }
}