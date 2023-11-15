// <copyright file="ISelectComponentRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using RSBingoBot.DiscordComponents;
using RSBingoBot.InteractionHandlers;

internal record ViewEvidenceSelectionRequest() : ISelectComponentRequest
{
    public SelectComponent SelectComponent { get; set; } = null!;

    public IInteractionHandler? ParentHandler { get; set; }

    public DiscordInteraction Interaction { get; set; } = null!;
}
