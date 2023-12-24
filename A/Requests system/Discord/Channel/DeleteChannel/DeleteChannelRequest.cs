// <copyright file="DeleteChannelRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

internal record DeleteChannelRequest(DiscordChannel Channel) : IDiscordRequest;