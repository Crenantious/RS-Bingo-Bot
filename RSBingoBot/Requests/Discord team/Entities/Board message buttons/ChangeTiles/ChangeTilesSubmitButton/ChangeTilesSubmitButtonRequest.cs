// <copyright file="ChangeTilesSubmitButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;

public record ChangeTilesSubmitButtonRequest(IDataWorker DataWorker, Team Team, ChangeTilesButtonDTO DTO, DiscordUser User,
    ChangeTilesTileSelect ChangeTilesTileSelect, ChangeTilesTaskSelect ChangeTilesTaskSelect,
    MessageFile BoardMessageFile) : IButtonRequest;