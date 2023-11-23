// <copyright file="RemoveUserFromTeamHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Models;
using System.Threading;

internal class RemoveUserFromTeamHandler : RequestHandler<RemoveUserFromTeamRequest>
{
    private const string UserSuccessfullyRemovedMessage = "The user '{0}' has been removed from the team.";
    private const string TeamRoleDoesNotExistWarning = "The team's role does not exist.";

    private static readonly SemaphoreSlim semaphore = new(1, 1);

    public RemoveUserFromTeamHandler() : base(semaphore)
    {

    }

    protected async override Task Process(RemoveUserFromTeamRequest request, CancellationToken cancellationToken)
    {
        User user = DataWorker.Users.FirstOrDefault(u => u.DiscordUserId == request.User.Id)!;
        DataWorker.Users.Remove(user);
        AddSuccess(UserSuccessfullyRemovedMessage.FormatConst(request.User.Username));

        DiscordRole? role = DataFactory.Guild.GetRole(user.Team.RoleId);
        if (role is null)
        {
            AddWarning(TeamRoleDoesNotExistWarning);
            return;
        }

        DiscordMember member = await DataFactory.Guild.GetMemberAsync(request.User.Id);
        await member.RevokeRoleAsync(role);
    }
}