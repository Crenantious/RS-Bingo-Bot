// <copyright file="UpdateMessageValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Validation;

internal class UpdateMessageValidator : Validator<UpdateMessageRequest>
{
    public UpdateMessageValidator()
    {
        NotNull(r => r.Message, "Message cannot be null");
    }
}