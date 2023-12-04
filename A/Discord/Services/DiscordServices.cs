// <copyright file="DiscordServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DSharpPlus;
using DSharpPlus.Entities;
using FluentResults;
using Microsoft.Extensions.Logging;
using RSBingo_Framework.DAL;
using RSBingoBot.Requests;

public class DiscordServices : IDiscordServices
{
    private readonly Logger<DiscordServices> logger;

    private DiscordServices(Logger<DiscordServices> logger)
    {
        this.logger = logger;
    }

    public async Task<Result<DiscordMember>> GetMember(ulong id) =>
        await RequestServices.Run<GetDiscordUserRequest, DiscordMember>(new GetDiscordUserRequest(id));

    public async Task<Result<DiscordRole>> CreateRole(string name) =>
        await RequestServices.Run<CreateRoleRequest, DiscordRole>(new CreateRoleRequest(name));

    public async Task<Result<DiscordRole>> GetRole(ulong id) =>
        await RequestServices.Run<GetRoleRequest, DiscordRole>(new GetRoleRequest(id));

    public async Task<Result> GrantRole(DiscordMember member, DiscordRole role) =>
        await RequestServices.Run(new GrantDiscordRoleRequest(member, role));

    public async Task<Result<DiscordChannel>> CreateChannel(string name, ChannelType channelType, DiscordChannel? parent = null,
        IEnumerable<DiscordOverwriteBuilder>? overwrites = null) =>
        await RequestServices.Run<CreateChannelRequest, DiscordChannel>(new CreateChannelRequest(name, channelType, parent, overwrites));

    public async Task<Result<DiscordChannel>> GetChannel(ulong id) =>
        await RequestServices.Run<GetChannelRequest, DiscordChannel>(new GetChannelRequest(id));

    public async Task<Result<DiscordChannel>> SendMessage(Message message, DiscordChannel channel) =>
        await RequestServices.Run(new SendMessageRequest(message, channel));

    public async Task<Result<Message>> GetMessage(ulong id, DiscordChannel channel) =>
        await RequestServices.Run<GetMessageRequest, Message>(new GetMessageRequest(id, channel));
}