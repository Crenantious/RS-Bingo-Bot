// <copyright file="SendMessageValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.Requests.Validation;

internal class SendMessageValidator : Validator<SendMessageRequest>
{
    public SendMessageValidator()
    {
        NotNull(r => r.Message, "Message");
        NotNull(r => r.Channel, "Channel");
    }
}