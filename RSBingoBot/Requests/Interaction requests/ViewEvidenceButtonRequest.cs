// <copyright file="ViewEvidenceButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using RSBingoBot.InteractionHandlers;

internal record ViewEvidenceButtonRequest(IInteractionHandler? ParentHandler) :
    IInteractionRequest
{
    public DiscordInteraction Interaction { get; set; } = null!;
}
