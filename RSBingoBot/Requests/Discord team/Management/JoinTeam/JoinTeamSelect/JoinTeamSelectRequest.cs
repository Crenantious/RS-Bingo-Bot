// <copyright file="JoinTeamSelectRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

public record JoinTeamSelectRequest(DiscordUser User) : ISelectComponentRequest
{
    public ComponentInteractionCreateEventArgs InteractionArgs { get; set; } = null!;
    public SelectComponent Component { get; set; } = null!;
}