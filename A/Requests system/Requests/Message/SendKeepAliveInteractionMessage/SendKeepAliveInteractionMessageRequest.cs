// <copyright file="SendKeepAliveInteractionMessageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DSharpPlus.Entities;

namespace DiscordLibrary.Requests;

public record SendKeepAliveInteractionMessageRequest(DiscordInteraction Interaction) : IDiscordRequest;