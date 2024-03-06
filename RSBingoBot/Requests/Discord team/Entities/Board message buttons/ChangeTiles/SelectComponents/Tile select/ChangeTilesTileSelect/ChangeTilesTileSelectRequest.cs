// <copyright file="ChangeTilesTileSelectRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;

public record ChangeTilesTileSelectRequest(ChangeTilesButtonDTO DTO) : ISelectComponentRequest;