// <copyright file="RemoveUserFromTeamHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;

internal class RemoveUserFromTeamHandler : RequestHandler<RemoveUserFromTeamRequest>
{
    private readonly IDatabaseServices databaseServices;
    private readonly IDiscordServices discordServices;

    public RemoveUserFromTeamHandler(IDatabaseServices databaseServices, IDiscordServices discordServices)
    {
        this.databaseServices = databaseServices;
        this.discordServices = discordServices;
    }

    protected override async Task Process(RemoveUserFromTeamRequest request, CancellationToken cancellationToken)
    {
        await RemoveFromDatabase(request);
        await RevokeRole(request);
    }

    private async Task RemoveFromDatabase(RemoveUserFromTeamRequest request)
    {
        IDataWorker dataWorker = DataFactory.CreateDataWorker();
        User user = dataWorker.Users.FirstOrDefault(u => u.DiscordUserId == request.User.Id)!;
        dataWorker.Users.Remove(user);
        await databaseServices.Update(dataWorker);
        AddSuccess(new RemoveUserFromTeamRemovedSuccess(request.User, request.DiscordTeam.Team));
    }

    private async Task RevokeRole(RemoveUserFromTeamRequest request)
    {
        Result<DiscordMember> member = await discordServices.GetMember(request.User.Id);
        if (member.IsFailed)
        {
            AddError(new RemoveUserFromTeamError());
            return;
        }

        Result revokeRole = await discordServices.RevokeRole(member.Value, request.DiscordTeam.Role!);
        if (revokeRole.IsFailed)
        {
            AddError(new RemoveUserFromTeamError());
        }
    }
}