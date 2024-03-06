// <copyright file="ChangeTilesTaskSelectRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;

public record ChangeTilesTaskSelectRequest(ChangeTilesButtonDTO DTO) : ISelectComponentRequest;