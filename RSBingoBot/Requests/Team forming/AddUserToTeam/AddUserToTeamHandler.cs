// <copyright file="AddUserToTeamHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;

internal class AddUserToTeamHandler : RequestHandler<AddUserToTeamRequest>
{
    private readonly IDiscordServices discordServices;

    public AddUserToTeamHandler(IDiscordServices discordServices)
    {
        this.discordServices = discordServices;
    }

    protected override async Task Process(AddUserToTeamRequest request, CancellationToken cancellationToken)
    {
        AddUserToDB(request);
        await GrantRole(request);
    }

    private void AddUserToDB(AddUserToTeamRequest request)
    {
        IDataWorker dataWorker = DataFactory.CreateDataWorker();
        dataWorker.Users.Create(request.User.Id, request.DiscordTeam.Team);
        dataWorker.SaveChanges();
        AddSuccess(new JoinTeamSelectAddedToTeamSuccess(request.User));
    }

    private async Task GrantRole(AddUserToTeamRequest request)
    {
        Result<DSharpPlus.Entities.DiscordMember> member = await discordServices.GetMember(request.User.Id);
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
    }
}