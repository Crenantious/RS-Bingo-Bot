// <copyright file="ChangeTilesToSelectRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;

public record ChangeTilesToSelectRequest(ChangeTilesButtonDTO DTO) : ISelectComponentRequest;