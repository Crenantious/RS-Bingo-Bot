// <copyright file="GetRoleHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using RSBingo_Framework.DAL;

internal class GetRoleHandler : RequestHandler<GetRoleRequest, DiscordRole>
{
    protected override async Task<DiscordRole> Process(GetRoleRequest request, CancellationToken cancellationToken)
    {
        DiscordRole role = DataFactory.Guild.GetRole(request.Id);
        AddSuccess(new GetRoleSuccess());
        return role;
    }
}