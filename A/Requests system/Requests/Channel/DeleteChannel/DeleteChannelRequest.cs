// <copyright file="DeleteChannelRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

public record DeleteChannelRequest(DiscordChannel Channel) : IDiscordRequest;