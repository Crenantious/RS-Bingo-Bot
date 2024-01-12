// <copyright file="SendMessageSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;

internal class SendMessageSuccess : MessageSuccess
{
    public SendMessageSuccess(Message message) :
        base("Sent a message.", message.DiscordMessage)
    {

    }
}