// <copyright file="IDiscordServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.Requests;
using DSharpPlus;
using DSharpPlus.Entities;
using FluentResults;

public interface IDiscordServices
{
    public Task<Result<DiscordMember>> GetMember(ulong id);

    public Task<Result<DiscordMember>> GetMember(DiscordUser user);

    public Task<Result<DiscordRole>> CreateRole(string name);

    public Task<Result<DiscordRole>> GetRole(ulong id);

    public Task<Result> GrantRole(DiscordMember member, DiscordRole role);

    public Task<Result> RevokeRole(DiscordMember member, DiscordRole role);

    public Task<Result> RenameRole(DiscordRole role, string newName);

    public Task<Result> DeleteRole(DiscordRole role);

    public Task<Result<DiscordChannel>> CreateChannel(string name, ChannelType channelType, DiscordChannel? parent = null,
        IEnumerable<DiscordOverwriteBuilder>? overwrites = null);

    public Task<Result<DiscordChannel>> CreateChannel(ChannelInfo channelInfo);

    public Task<Result<DiscordChannel>> GetChannel(ulong id);

    public Task<Result> RenameChannel(DiscordChannel channel, string newName);

    public Task<Result> DeleteChannel(DiscordChannel channel);
}