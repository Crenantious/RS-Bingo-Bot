// <copyright file="ChangeTilesSubmitButtonAddedTileToBoardSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Framework.Models;

internal class ChangeTilesSubmitButtonAddedTileToBoardSuccess : Success, IDiscordResponse
{
    private const string SuccessMessage = "Added '{0}' to the board.";

    public ChangeTilesSubmitButtonAddedTileToBoardSuccess(Tile tile) : base(SuccessMessage.FormatConst(tile.Task.Name))
    {

    }
}