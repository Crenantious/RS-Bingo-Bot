// <copyright file="DiscordServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.Requests;
using DSharpPlus;
using DSharpPlus.Entities;
using FluentResults;
using RSBingoBot.Requests;

public class DiscordServices : IDiscordServices
{
    public async Task<Result<DiscordMember?>> GetMember(ulong id) =>
        await RequestRunner.Run<GetDiscordMemberRequest, DiscordMember?>(new GetDiscordMemberRequest(id));

    public async Task<Result<DiscordRole>> CreateRole(string name) =>
        await RequestRunner.Run<CreateRoleRequest, DiscordRole>(new CreateRoleRequest(name));

    public async Task<Result<DiscordRole>> GetRole(ulong id) =>
        await RequestRunner.Run<GetRoleRequest, DiscordRole>(new GetRoleRequest(id));

    public async Task<Result> DeleteRole(DiscordRole role) =>
        await RequestRunner.Run(new DeleteRoleRequest(role));

    public async Task<Result> GrantRole(DiscordMember member, DiscordRole role) =>
        await RequestRunner.Run(new GrantDiscordRoleRequest(member, role));

    public async Task<Result<DiscordChannel>> CreateChannel(string name, ChannelType channelType, DiscordChannel? parent = null,
        IEnumerable<DiscordOverwriteBuilder>? overwrites = null) =>
        await RequestRunner.Run<CreateChannelRequest, DiscordChannel>(new CreateChannelRequest(name, channelType, parent, overwrites));

    public Task<Result<DiscordChannel>> CreateChannel(ChannelInfo info) =>
        CreateChannel(info.Name, info.ChannelType, info.Parent, info.Overwrites);

    public async Task<Result<DiscordChannel>> GetChannel(ulong id) =>
        await RequestRunner.Run<GetChannelRequest, DiscordChannel>(new GetChannelRequest(id));

    public async Task<Result> DeleteChannel(DiscordChannel channel) =>
        await RequestRunner.Run(new DeleteChannelRequest(channel));
}