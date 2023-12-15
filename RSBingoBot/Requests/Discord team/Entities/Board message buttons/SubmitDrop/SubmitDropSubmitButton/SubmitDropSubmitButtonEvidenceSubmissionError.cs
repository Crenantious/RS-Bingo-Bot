// <copyright file="SubmitDropSubmitButtonEvidenceSubmissionError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Framework.Models;

internal class SubmitDropSubmitButtonEvidenceSubmissionError : Error, IDiscordResponse
{
    private const string ErrorMessage = "Unable to submit drop for {0}.";

    public SubmitDropSubmitButtonEvidenceSubmissionError(Tile tile) : base(ErrorMessage.FormatConst(tile.Task.Name))
    {

    }
}