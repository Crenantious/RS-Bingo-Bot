// <copyright file="SubmitEvidenceMessageValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Extensions;
using RSBingoBot.Requests.Validation;

internal class SubmitEvidenceMessageValidator : BingoValidator<SubmitEvidenceMessageRequest>
{
    public SubmitEvidenceMessageValidator()
    {
        DiscordMessageExists(r => r.GetMessage());
        IsImage(r => r.GetMessage().DiscordMessage.Attachments.ElementAt(0));
    }
}