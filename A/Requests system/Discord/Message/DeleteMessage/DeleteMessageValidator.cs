// <copyright file="DeleteMessageValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Validation;

internal class DeleteMessageValidator : Validator<DeleteMessageRequest>
{
    public DeleteMessageValidator()
    {
        NotNull(r => r.Message, "Message");
        DiscordMessageExists(r => r.Message);
    }
}