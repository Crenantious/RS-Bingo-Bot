// <copyright file="DiscordServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DSharpPlus.Entities;
using FluentResults;
using Microsoft.Extensions.Logging;
using RSBingoBot.Requests;

// TODO: JR - convert other Discord services to requests and wrap them here.
public class DiscordServices : IDiscordServices
{
    private readonly Logger<DiscordServices> logger;

    private DiscordServices(Logger<DiscordServices> logger)
    {
        this.logger = logger;
    }

    public async Task<Result<DiscordMember>> GetUser(ulong id) =>
        await RequestServices.Run<GetDiscordUserRequest, DiscordMember>(new GetDiscordUserRequest(id));

    public async Task<Result<DiscordRole>> CreateRole(string name) =>
        await RequestServices.Run<CreateRoleRequest, DiscordRole>(new CreateRoleRequest(name));

    public async Task<Result> GrantRole(DiscordMember member, DiscordRole role) =>
        await RequestServices.Run(new GrantDiscordRoleRequest(member, role));
}