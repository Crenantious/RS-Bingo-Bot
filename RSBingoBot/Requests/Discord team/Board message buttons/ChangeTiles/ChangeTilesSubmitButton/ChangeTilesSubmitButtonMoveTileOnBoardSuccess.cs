// <copyright file="ChangeTilesSubmitButtonMoveTileOnBoardSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using RSBingo_Framework.Models;

internal class ChangeTilesSubmitButtonMoveTileOnBoardSuccess : Success
{
    private const string SuccessMessage = "Moved '{0}' on the board.";

    public ChangeTilesSubmitButtonMoveTileOnBoardSuccess(Tile tile) : base(SuccessMessage.FormatConst(tile.Task.Name))
    {

    }
}