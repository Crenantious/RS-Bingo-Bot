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
    private readonly IDiscordUserServices discordUserServices;

    public AssignTeamRoleHandler(IDiscordUserServices discordUserServices)
    {
        this.discordUserServices = discordUserServices;
    }

    protected override async Task Process(AssignTeamRoleRequest request, CancellationToken cancellationToken)
    {
        foreach (User user in request.Team.Users)
        {
            Result<DiscordMember> discordUser = await discordUserServices.GetUser(user.DiscordUserId);
            if (discordUser.IsSuccess)
            {
                await discordUser.Value.GrantRoleAsync(request.Role);
            }
        }
    }
}