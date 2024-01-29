// <copyright file="ChangeTilesSubmitButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;

public record ChangeTilesSubmitButtonRequest(int TeamId, ChangeTilesButtonDTO DTO) : IButtonRequest;