// <copyright file="ChangeTilesTaskSelectBackButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;

internal record ChangeTilesTaskSelectBackButtonRequest(SelectComponent SelectComponent, ChangeTilesButtonDTO DTO) :
    SelectComponentBackButtonRequest(SelectComponent);