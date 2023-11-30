// <copyright file="AddUserToTeamHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.RequestHandlers;
using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;

internal class AddUserToTeamHandler : InteractionHandler<AddUserToTeamRequest, Button>
{
    private readonly IDiscordServices discordServices;
    private IDataWorker dataWorker = DataFactory.CreateDataWorker();

    public AddUserToTeamHandler(IDiscordServices discordServices)
    {
        this.discordServices = discordServices;
    }

    protected override async Task Process(AddUserToTeamRequest request, CancellationToken cancellationToken)
    {
        dataWorker.Users.Create(request.User.Id, request.DiscordTeam.Team);
        dataWorker.SaveChanges();
        AddSuccess(new AddUserToTeamAddedToTeamSuccess(request.User));

        Result<DiscordMember> member = await discordServices.GetMember(request.User.Id);
        if (member.IsSuccess)
        {
            await discordServices.GrantRole(member.Value, request.DiscordTeam.Role!);
            AddSuccess(new AddUserToTeamAddedRoleSuccess(request.User));
        }
    }
}