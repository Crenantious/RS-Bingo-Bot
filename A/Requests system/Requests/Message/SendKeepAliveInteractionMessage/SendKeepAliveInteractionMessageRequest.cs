// <copyright file="SendKeepAliveInteractionMessageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

public record SendKeepAliveInteractionMessageRequest(DiscordInteraction Interaction, bool IsEphemeral) : IDiscordRequest;