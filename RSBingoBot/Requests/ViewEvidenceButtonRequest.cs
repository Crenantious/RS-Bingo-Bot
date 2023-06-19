// <copyright file="ViewEvidenceButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Framework.Models;
using RSBingoBot.Interfaces;

internal record ViewEvidenceButtonRequest(User User) : IInteractionRequest<Result>
{
    public DiscordInteraction DiscordInteraction { get; set; }
}