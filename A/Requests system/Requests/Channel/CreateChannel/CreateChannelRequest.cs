// <copyright file="CreateChannelRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus;
using DSharpPlus.Entities;

public record CreateChannelRequest(string Name, ChannelType ChannelType, DiscordChannel? Parent = null,
        IEnumerable<DiscordOverwriteBuilder>? Overwrites = null) : IDiscordRequest<DiscordChannel>;