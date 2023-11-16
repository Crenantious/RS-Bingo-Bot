// <copyright file="ISelectComponentRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.RequestHandlers;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

internal record ViewEvidenceSelectionRequest() : ISelectComponentRequest
{
    public IInteractionHandler? ParentHandler { get; set; }

    public DiscordInteraction Interaction { get; set; } = null!;
}