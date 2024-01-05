// <copyright file="DeleteMessageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

public record DeleteMessageRequest(DiscordMessage Message) : IDiscordRequest;