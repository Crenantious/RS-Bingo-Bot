// <copyright file="SubmitDropSubmitButtonEvidenceReviewMessageError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Framework.Models;

internal class SubmitDropSubmitButtonEvidenceReviewMessageError : Error, IDiscordResponse
{
    private const string ErrorMessage = "Unable to submit drop for {0}.";

    public SubmitDropSubmitButtonEvidenceReviewMessageError(Tile tile) : base(ErrorMessage.FormatConst(tile.Task.Name))
    {

    }
}