// <copyright file="ChangeTilesTileSelectBackButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;

internal record ChangeTilesTileSelectBackButtonRequest(SelectComponent SelectComponent, ChangeTilesButtonDTO DTO) :
    SelectComponentBackButtonRequest(SelectComponent);