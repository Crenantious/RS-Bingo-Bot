// <copyright file="JoinTeamButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

internal record JoinTeamButtonRequest(DiscordUser User) : IButtonRequest
{
    public InteractionCreateEventArgs InteractionArgs { get; set; } = null!;
    public Button Component { get; set; }
}