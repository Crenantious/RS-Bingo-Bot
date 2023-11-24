﻿// <copyright file="IDiscordServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DSharpPlus;
using DSharpPlus.Entities;
using FluentResults;

public interface IDiscordServices
{
    public Task<Result<DiscordMember>> GetUser(ulong id);

    public Task<Result<DiscordRole>> CreateRole(string name);

    public Task<Result> GrantRole(DiscordMember member, DiscordRole role);

    public Task<Result<DiscordChannel>> CreateChannel(string name, ChannelType channelType, DiscordChannel? parent = null,
        IEnumerable<DiscordOverwriteBuilder>? overwrites = null);

    public Task<Result<DiscordChannel>> GetChannel(ulong id);

}