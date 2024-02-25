// <copyright file="ChangeTilesSubmitButtonAddedTileToBoardSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Framework.Models;

internal class ChangeTilesSubmitButtonAddedTileToBoardSuccess : Success, IDiscordResponse
{
    private const string SuccessMessage = "Added '{0}' to position {1}.";

    public ChangeTilesSubmitButtonAddedTileToBoardSuccess(Tile tile) : base(SuccessMessage.FormatConst(tile.Task.Name, tile.BoardIndex))
    {

    }
}