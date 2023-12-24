// <copyright file="CreateRoleHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;
using RSBingo_Framework.DAL;

internal class CreateRoleHandler : DiscordHandler<CreateRoleRequest, DiscordRole>
{
    protected override async Task<DiscordRole> Process(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        DiscordRole role = await DataFactory.Guild.CreateRoleAsync(request.Name);
        AddSuccess(new CreateRoleSuccess(role));
        return role;
    }
}