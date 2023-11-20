// <copyright file="ViewEvidenceButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.RequestHandlers;
using DiscordLibrary.Requests;
using DSharpPlus.EventArgs;
using RSBingo_Framework.Models;

internal record ViewEvidenceButtonRequest(Team Team, IInteractionHandler? ParentHandler) : IButtonRequest
{
    public Button Component { get; set; } = null!;

    public InteractionCreateEventArgs InteractionArgs { get; set; } = null!;
}