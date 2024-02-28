// <copyright file="SendMessageValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.Requests.Validation;

internal class SendMessageValidator : Validator<SendMessageRequest>
{
    public SendMessageValidator()
    {
        NotNull(r => r.Message, "Message cannot be null");
        NotNull(r => r.Message.Channel, "Channel cannot be null");
    }
}