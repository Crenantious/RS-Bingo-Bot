// <copyright file="GrantDiscordRoleHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;

internal class GrantDiscordRoleHandler : DiscordHandler<GrantDiscordRoleRequest>
{
    private readonly IDiscordServices userServices;

    public GrantDiscordRoleHandler(IDiscordServices userServices)
    {
        this.userServices = userServices;
    }

    protected override async Task Process(GrantDiscordRoleRequest request, CancellationToken cancellationToken)
    {
        await request.Member.GrantRoleAsync(request.Role);
        AddSuccess(new GrantDiscordRoleSuccess());
    }
}