// <copyright file="RenameChannelRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

internal record RenameChannelRequest(DiscordChannel Channel, string NewName) : IDiscordRequest;