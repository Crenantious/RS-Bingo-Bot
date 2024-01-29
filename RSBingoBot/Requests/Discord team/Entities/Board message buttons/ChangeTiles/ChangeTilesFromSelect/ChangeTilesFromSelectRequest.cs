// <copyright file="ChangeTilesFromSelectRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;

public record ChangeTilesFromSelectRequest(ChangeTilesButtonDTO DTO) : ISelectComponentRequest;