// <copyright file="DiscordServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.Requests;
using DSharpPlus;
using DSharpPlus.Entities;
using FluentResults;
using RSBingoBot.Requests;

public class DiscordServices : RequestService, IDiscordServices
{
    public async Task<Result<DiscordMember>> GetMember(ulong id) =>
        await RunRequest<GetDiscordMemberRequest, DiscordMember>(new GetDiscordMemberRequest(id));

    #region role

    public async Task<Result<DiscordRole>> CreateRole(string name) =>
        await RunRequest<CreateRoleRequest, DiscordRole>(new CreateRoleRequest(name));

    public async Task<Result<DiscordRole>> GetRole(ulong id) =>
        await RunRequest<GetRoleRequest, DiscordRole>(new GetRoleRequest(id));

    public async Task<Result> RenameRole(DiscordRole role, string newName) =>
        await RunRequest(new RenameRoleRequest(role, newName));

    public async Task<Result> GrantRole(DiscordMember member, DiscordRole role) =>
        await RunRequest(new GrantDiscordRoleRequest(member, role));

    public async Task<Result> RevokeRole(DiscordMember member, DiscordRole role) =>
        await RunRequest(new RevokeRoleRequest(member, role));

    public async Task<Result> DeleteRole(DiscordRole role) =>
        await RunRequest(new DeleteRoleRequest(role));

    #endregion

    #region channel

    public async Task<Result<DiscordChannel>> CreateChannel(string name, ChannelType channelType, DiscordChannel? parent = null,
        IEnumerable<DiscordOverwriteBuilder>? overwrites = null) =>
        await RunRequest<CreateChannelRequest, DiscordChannel>(new CreateChannelRequest(name, channelType, parent, overwrites));

    public Task<Result<DiscordChannel>> CreateChannel(ChannelInfo info) =>
        CreateChannel(info.Name, info.ChannelType, info.Parent, info.Overwrites);

    public async Task<Result<DiscordChannel>> GetChannel(ulong id) =>
        await RunRequest<GetChannelRequest, DiscordChannel>(new GetChannelRequest(id));

    public async Task<Result> RenameChannel(DiscordChannel channel, string newName) =>
        await RunRequest(new RenameChannelRequest(channel, newName));

    public async Task<Result> DeleteChannel(DiscordChannel channel) =>
        await RunRequest(new DeleteChannelRequest(channel));

    #endregion
}