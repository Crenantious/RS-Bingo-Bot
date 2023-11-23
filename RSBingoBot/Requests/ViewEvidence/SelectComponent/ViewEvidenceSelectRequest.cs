﻿// <copyright file="ViewEvidenceSelectRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;
using DSharpPlus.EventArgs;

internal record ViewEvidenceSelectRequest() : ISelectComponentRequest
{
    public SelectComponent Component { get; set; } = null!;

    public InteractionCreateEventArgs InteractionArgs { get; set; } = null!;
}