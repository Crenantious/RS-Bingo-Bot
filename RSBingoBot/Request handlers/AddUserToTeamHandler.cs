// <copyright file="AddUserToTeamRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using RSBingo_Framework.DAL;

internal class AddUserToTeamHandler : RequestHandlerBase<AddUserToTeamRequest>
{
    private const string UserSuccessfullyAddedMessage = "The user '{0}' has been added to the team successfully.";
    private const string TeamRoleDoesNotExistError = "The team's role does not exist.";

    // TODO: JR - consider having one semaphore for all database write requests to avoid conflicts.
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    public AddUserToTeamHandler() : base(semaphore)
    {

    }

    protected override async Task Process(AddUserToTeamRequest request, CancellationToken cancellationToken)
    {
        ulong userId = request.DiscordUser.Id;
        string teamName = request.TeamName;

        // TODO: JR - consider what happens if the team is deleted/renamed between validation and here.
        DiscordRole? role = RequestsUtilities.GetTeamRole(DataWorker, teamName);
        DataWorker.Users.Create(userId, teamName);

        if (role is null)
        {
            // TODO: JR - create the team role and log a warning.
        }

        DiscordMember member = await DataFactory.Guild.GetMemberAsync(userId);
        await member.GrantRoleAsync(role);
    }
}