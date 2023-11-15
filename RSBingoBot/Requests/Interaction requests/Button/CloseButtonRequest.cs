// <copyright file="CloseButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using RSBingoBot.InteractionHandlers;

// TODO: JR - consider running this through another request ConcludeInteractionRequest, possibly in conjunction.
internal record CloseButtonRequest(IInteractionHandler? ParentHandler) : IButtonRequest
{
    public DiscordInteraction Interaction { get; set; } = null!;
}