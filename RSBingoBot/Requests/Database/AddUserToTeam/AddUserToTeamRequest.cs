// <copyright file="AddUserToTeamRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

internal record AddUserToTeamRequest(DiscordUser User, RSBingoBot.Discord.DiscordTeam DiscordTeam) : IInteractionRequest
{
    public InteractionCreateEventArgs InteractionArgs { get; set; } = null!;
}