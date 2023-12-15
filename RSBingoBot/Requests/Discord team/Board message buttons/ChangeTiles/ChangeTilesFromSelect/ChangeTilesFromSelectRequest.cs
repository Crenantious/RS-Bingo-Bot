﻿// <copyright file="ChangeTilesFromSelectRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;
using DSharpPlus.EventArgs;

internal record ChangeTilesFromSelectRequest(ChangeTilesButtonDTO DTO) : ISelectComponentRequest
{
    public SelectComponent Component { get; set; } = null!;
    public ComponentInteractionCreateEventArgs InteractionArgs { get; set; } = null!;
}