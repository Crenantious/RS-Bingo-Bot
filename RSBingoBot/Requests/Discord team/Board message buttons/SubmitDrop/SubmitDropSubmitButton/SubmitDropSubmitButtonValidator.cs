// <copyright file="SubmitDropSubmitButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.Requests.Validation;

internal class SubmitDropSubmitButtonValidator : Validator<SubmitDropSubmitButtonRequest>
{
    private const string NoTilesSelectedError = "At least one tile must be selected to submit evidence for.";
    private const string NoEvidenceSubmittedError = "You cannot submit no evidence; please post a message with a single image first.";

    public SubmitDropSubmitButtonValidator()
    {
        NotNull(r => r.GetTile(), NoTilesSelectedError);
        NotNull(r => r.GetUrl(), NoEvidenceSubmittedError);
    }
}