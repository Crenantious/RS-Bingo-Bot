// <copyright file="CreateTeamRoleHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;

internal class CreateTeamRoleHandler : DiscordHandler<CreateTeamRoleRequest, DiscordRole>
{
    protected override async Task<DiscordRole> Process(CreateTeamRoleRequest request, CancellationToken cancellationToken)
    {
        var userServices = GetRequestService<IDiscordServices>();

        Result<DiscordRole> role = await userServices.CreateRole(request.DiscordTeam.RoleName);
        if (role.IsSuccess)
        {
            AddSuccess(new CreateTeamRoleSuccess());
            return role.Value;
        }
        else
        {
            AddError(new CreateTeamRoleError());
            return null!;
        }
    }
}