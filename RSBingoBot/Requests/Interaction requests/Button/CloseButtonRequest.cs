// <copyright file="CloseButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using DSharpPlus.Entities;

internal record CloseButtonRequest(IInteractionHandler) : IButtonRequest
{
    public DiscordInteraction Interaction { get; set; } = null!;
}