// <copyright file="GrantDiscordRoleHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

internal class GrantDiscordRoleHandler : DiscordHandler<GrantDiscordRoleRequest>
{
    protected override async Task Process(GrantDiscordRoleRequest request, CancellationToken cancellationToken)
    {
        await request.Member.GrantRoleAsync(request.Role);
        AddSuccess(new GrantDiscordRoleSuccess(request.Role, request.Member));
    }
}