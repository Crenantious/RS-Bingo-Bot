// <copyright file="SendMessageError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

internal class SendMessageError : MessageError
{
    public SendMessageError(string prefix, Message message, DiscordChannel channel) :
        base(prefix, message, channel)
    {

    }
}