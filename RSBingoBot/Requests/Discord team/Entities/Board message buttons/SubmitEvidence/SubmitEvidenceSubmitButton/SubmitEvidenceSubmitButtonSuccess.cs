// <copyright file="SubmitEvidenceSubmitButtonSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Framework.Models;

internal class SubmitEvidenceSubmitButtonSuccess : Success, IDiscordResponse
{
    private const string SuccessMessage = "Evidence submitted for {0}.";

    public SubmitEvidenceSubmitButtonSuccess(Tile tile) : base(SuccessMessage.FormatConst(tile.Task.Name))
    {

    }
}