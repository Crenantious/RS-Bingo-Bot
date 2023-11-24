// <copyright file="DiscordUserServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DSharpPlus.Entities;
using FluentResults;
using Microsoft.Extensions.Logging;
using RSBingoBot.Requests;

public class DiscordUserServices : IDiscordUserServices
{
    private readonly Logger<DiscordUserServices> logger;

    private DiscordUserServices(Logger<DiscordUserServices> logger)
    {
        this.logger = logger;
    }

    public async Task<Result<DiscordMember>> GetUser(ulong id) =>
        await RequestServices.Run<GetDiscordUserRequest, DiscordMember>(new GetDiscordUserRequest(id));

    public async Task<Result> GrantRole(DiscordMember member, DiscordRole role) =>
        await RequestServices.Run(new GrantDiscordRoleRequest(member, role));
}