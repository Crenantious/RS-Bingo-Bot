// <copyright file="SubmitDropMessageValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.Requests.Validation;

internal class SubmitDropMessageValidator : BingoValidator<SubmitDropMessageRequest>
{
    public SubmitDropMessageValidator()
    {
        DiscordMessageExists(r => r.Message);
        IsImage(r => r.MessageArgs.Message.Attachments.ElementAt(0));
    }
}