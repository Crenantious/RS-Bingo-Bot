// <copyright file="SubmitEvidenceSubmitButtonTileAlreadyCompleteError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Framework.Models;

internal class SubmitEvidenceSubmitButtonTileAlreadyCompleteError : Error, IDiscordResponse
{
    private const string ErrorMessage = "The tile {0} has already been completed.";

    public SubmitEvidenceSubmitButtonTileAlreadyCompleteError(Tile tile) : base(ErrorMessage.FormatConst(tile.Task.Name))
    {

    }
}