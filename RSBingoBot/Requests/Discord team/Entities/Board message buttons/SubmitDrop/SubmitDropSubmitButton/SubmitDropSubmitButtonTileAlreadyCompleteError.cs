// <copyright file="SubmitDropSubmitButtonTileAlreadyCompleteError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Framework.Models;

internal class SubmitDropSubmitButtonTileAlreadyCompleteError : Error, IDiscordResponse
{
    private const string ErrorMessage = "The tile {0} has already been completed.";

    public SubmitDropSubmitButtonTileAlreadyCompleteError(Tile tile) : base(ErrorMessage.FormatConst(tile.Task.Name))
    {

    }
}