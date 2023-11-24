// <copyright file="AssignTeamRoleHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Framework.Models;

internal class AssignTeamRoleHandler : RequestHandler<AssignTeamRoleRequest>
{
    private readonly IDiscordServices discordUserServices;

    public AssignTeamRoleHandler(IDiscordServices discordUserServices)
    {
        this.discordUserServices = discordUserServices;
    }

    protected override async Task Process(AssignTeamRoleRequest request, CancellationToken cancellationToken)
    {
        foreach (User user in request.Team.Users)
        {
            Result<DiscordMember> discordMember = await discordUserServices.GetUser(user.DiscordUserId);
            if (discordMember.IsSuccess)
            {
                await discordUserServices.GrantRole(discordMember.Value, request.Role);
            }
        }
        AddSuccess(new AssignTeamRoleSuccess());
    }
}