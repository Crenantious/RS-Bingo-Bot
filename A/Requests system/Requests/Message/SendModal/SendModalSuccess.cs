// <copyright file="SendModalSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;

internal class SendModalSuccess : MessageSuccess
{
    private const string SuccessMessage = "Sent a modal";

    public SendModalSuccess(Modal modal) :
        base(SuccessMessage, modal.DiscordMessage)
    {

    }
}