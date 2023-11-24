// <copyright file="CreateTeamRoleHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using RSBingo_Framework.DAL;

internal class CreateTeamRoleHandler : RequestHandler<CreateTeamRoleRequest, DiscordRole>
{
    protected override async Task<DiscordRole> Process(CreateTeamRoleRequest request, CancellationToken cancellationToken)
    {
        DiscordRole role = await DataFactory.Guild.CreateRoleAsync(request.Team.Name);
        AddSuccess(new CreateTeamRoleSuccess());
        return role;
    }
}