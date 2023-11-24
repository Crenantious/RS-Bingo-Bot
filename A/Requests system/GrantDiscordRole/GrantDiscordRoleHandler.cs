// <copyright file="GrantDiscordRoleHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;

// TODO: JR - add a DiscordRequestHandler that catches for Discord exceptions and creates appropriate errors.
internal class GrantDiscordRoleHandler : RequestHandler<GrantDiscordRoleRequest>
{
    private readonly IDiscordUserServices userServices;

    public GrantDiscordRoleHandler(IDiscordUserServices userServices)
    {
        this.userServices = userServices;
    }

    protected override async Task Process(GrantDiscordRoleRequest request, CancellationToken cancellationToken)
    {
        await request.Member.GrantRoleAsync(request.Role);
        AddSuccess(new GrantDiscordRoleSuccess());
    }
}