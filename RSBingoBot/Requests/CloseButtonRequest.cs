// <copyright file="CloseButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using RSBingoBot.Interaction_handlers;
using RSBingoBot.Interfaces;

internal record CloseButtonRequest(IInteractionHandler InteractionHandler) : IInteractionRequest<Result>
{
    public DiscordInteraction DiscordInteraction { get; set; }
}