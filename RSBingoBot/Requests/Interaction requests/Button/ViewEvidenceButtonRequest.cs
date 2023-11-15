// <copyright file="ViewEvidenceButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using RSBingo_Framework.Models;
using RSBingoBot.InteractionHandlers;

internal record ViewEvidenceButtonRequest(Team Team, IInteractionHandler? ParentHandler) :
    IButtonRequest
{
    public DiscordInteraction Interaction { get; set; } = null!;
}
