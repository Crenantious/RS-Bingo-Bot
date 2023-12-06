// <copyright file="SubmitDropSubmitButtonSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;

internal class SubmitDropSubmitButtonEvidenceReviewMessageError : Error, IDiscordResponse
{
    private const string ErrorMessage = "Unable to submit drop, please try again or contact the bot manager if this persists.";

    public SubmitDropSubmitButtonEvidenceReviewMessageError() : base(ErrorMessage)
    {

    }
}