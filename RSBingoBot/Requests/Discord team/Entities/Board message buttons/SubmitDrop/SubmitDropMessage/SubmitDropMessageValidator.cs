// <copyright file="SubmitDropMessageValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Extensions;
using RSBingoBot.Requests.Validation;

internal class SubmitDropMessageValidator : BingoValidator<SubmitDropMessageRequest>
{
    public SubmitDropMessageValidator()
    {
        DiscordMessageExists(r => r.GetMessage());
        IsImage(r => r.GetMessage().DiscordMessage.Attachments.ElementAt(0));
    }
}