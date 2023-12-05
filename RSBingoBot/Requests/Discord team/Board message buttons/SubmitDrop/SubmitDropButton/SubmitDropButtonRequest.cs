// <copyright file="SubmitDropButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;
using DSharpPlus.EventArgs;
using RSBingoBot.Discord;

internal record SubmitDropButtonRequest(DiscordTeam DiscordTeam) : IButtonRequest
{
    public Button Component { get; set; } = null!;
    public ComponentInteractionCreateEventArgs InteractionArgs { get; set; } = null!;
}