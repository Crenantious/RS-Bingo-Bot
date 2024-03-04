// <copyright file="RemoveUserFromTeamHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using FluentResults;

internal class RemoveUserFromTeamHandler : RequestHandler<RemoveUserFromTeamRequest>
{
    private IDatabaseServices databaseServices = null!;
    private IDiscordServices discordServices = null!;

    protected override async Task Process(RemoveUserFromTeamRequest request, CancellationToken cancellationToken)
    {
        databaseServices = GetRequestService<IDatabaseServices>();
        discordServices = GetRequestService<IDiscordServices>();

        await RemoveFromDatabase(request);
        await RevokeRole(request);
    }

    private async Task RemoveFromDatabase(RemoveUserFromTeamRequest request)
    {
        request.DataWorker.Users.Remove(request.User);
        await databaseServices.SaveChanges(request.DataWorker);
        AddSuccess(new RemoveUserFromTeamRemovedSuccess(request.Member, request.DiscordTeam.Name));
    }

    private async Task RevokeRole(RemoveUserFromTeamRequest request)
    {
        Result revokeRole = await discordServices.RevokeRole(request.Member, request.DiscordTeam.Role!);
        if (revokeRole.IsFailed)
        {
            AddError(new RemoveUserFromTeamError());
        }
    }
}