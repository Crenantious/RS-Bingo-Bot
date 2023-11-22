// <copyright file="ConclueInteractionButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.RequestHandlers;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

internal record ConclueInteractionButtonRequest(IInteractionHandler handler) : IButtonRequest
{
    public DiscordInteraction Interaction { get; set; } = null!;
    public Button Component { get; set; } = null!;
    public InteractionCreateEventArgs InteractionArgs { get; set; } = null!;
}