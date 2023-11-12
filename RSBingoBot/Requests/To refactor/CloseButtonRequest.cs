// <copyright file="CloseButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using RSBingoBot.Interaction_handlers;
using RSBingoBot.Interfaces;

// TODO: JR - consider running this through another request ConcludeInteractionRequest, possibly in conjunction.
internal record CloseButtonRequest(IInteractionHandler InteractionHandler) : IInteractionRequest<Result>
{
    public DiscordInteraction DiscordInteraction { get; set; }
}