// <copyright file="JoinTeamSelectHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.RequestHandlers;
using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;

internal class JoinTeamSelectHandler : SelectComponentHandler<JoinTeamSelectRequest>
{
    private readonly IDiscordServices discordServices;
    private IDataWorker dataWorker = DataFactory.CreateDataWorker();

    public JoinTeamSelectHandler(IDiscordServices discordServices)
    {
        this.discordServices = discordServices;
    }

    protected override async Task Process(JoinTeamSelectRequest request, CancellationToken cancellationToken)
    {
        dataWorker.Users.Create(request.User.Id, request.DiscordTeam.Team);
        dataWorker.SaveChanges();
        AddSuccess(new JoinTeamSelectAddedToTeamSuccess(request.User), false);

        Result<DiscordMember> member = await discordServices.GetMember(request.User.Id);

        if (member.IsFailed)
        {
            AddErrors(member.Errors);
            return;
        }

        Result result = await discordServices.GrantRole(member.Value, request.DiscordTeam.Role!);
        if (result.IsFailed)
        {
            AddErrors(result.Errors);
            return;
        }

        AddSuccess(new JoinTeamSelectSuccess(request.DiscordTeam.Team));
    }
}