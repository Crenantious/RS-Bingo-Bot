// <copyright file="AddUserToTeamHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using FluentResults;

internal class AddUserToTeamHandler : RequestHandler<AddUserToTeamRequest>
{
    protected override async Task Process(AddUserToTeamRequest request, CancellationToken cancellationToken)
    {
        AddUserToDB(request);
        await GrantRole(request);
    }

    private void AddUserToDB(AddUserToTeamRequest request)
    {
        request.DataWorker.Users.Create(request.User.Id, request.DiscordTeam.Team);
        AddSuccess(new AddUserToTeamSuccess(request.DiscordTeam));
    }

    private async Task GrantRole(AddUserToTeamRequest request)
    {
        var discordServices = GetRequestService<IDiscordServices>();

        Result<DSharpPlus.Entities.DiscordMember> member = await discordServices.GetMember(request.User.Id);
        if (member.IsFailed)
        {
            return;
        }

        await discordServices.GrantRole(member.Value, request.DiscordTeam.Role!);
    }
}