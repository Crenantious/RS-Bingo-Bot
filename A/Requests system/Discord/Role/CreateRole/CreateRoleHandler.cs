// <copyright file="CreateRoleHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using RSBingo_Framework.DAL;

internal class CreateRoleHandler : RequestHandler<CreateRoleRequest, DiscordRole>
{
    protected override async Task<DiscordRole> Process(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var role = await DataFactory.Guild.CreateRoleAsync(request.Name);
        AddSuccess(new CreateRoleSuccess());
        return role;
    }
}