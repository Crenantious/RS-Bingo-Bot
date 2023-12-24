// <copyright file="ConcludeInteractionButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordComponents;
using DSharpPlus.EventArgs;

public record ConcludeInteractionButtonRequest(IInteractionHandler handler) : IButtonRequest
{
    public Button Component { get; set; } = null!;
    public ComponentInteractionCreateEventArgs InteractionArgs { get; set; } = null!;
}