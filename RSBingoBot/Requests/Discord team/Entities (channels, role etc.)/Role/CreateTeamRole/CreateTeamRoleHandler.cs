﻿// <copyright file="CreateTeamRoleHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;

internal class CreateTeamRoleHandler : DiscordHandler<CreateTeamRoleRequest>
{
    private readonly IDiscordServices userServices;

    public CreateTeamRoleHandler(IDiscordServices discordServices)
    {
        this.userServices = discordServices;
    }

    protected override async Task Process(CreateTeamRoleRequest request, CancellationToken cancellationToken)
    {
        Result<DiscordRole> role = await userServices.CreateRole(request.DiscordTeam.Team.Name);
        if (role.IsSuccess)
        {
            AddSuccess(new CreateTeamRoleSuccess());
        }
        else
        {
            AddError(new CreateTeamRoleError());
        }
    }
}