// <copyright file="ChangeTilesSubmitButtonMoveTileOnBoardSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Framework.Models;

internal class ChangeTilesSubmitButtonMoveTileOnBoardSuccess : Success, IDiscordResponse
{
    private const string SuccessMessage = "Moved '{0}' from position {1} to {2}.";

    public ChangeTilesSubmitButtonMoveTileOnBoardSuccess(Tile tile, int oldBoardIndex, int newBoardIndex) :
        base(SuccessMessage.FormatConst(tile.Task.Name, oldBoardIndex, newBoardIndex))
    {

    }
}