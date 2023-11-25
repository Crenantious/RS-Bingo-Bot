// <copyright file="MessageError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DSharpPlus.Entities;
using FluentResults;

internal class MessageError : Error
{
    public MessageError(string prefix, Message message, DiscordChannel channel) :
        base(prefix + MessageRequestInformation.Get(message, channel))
    {

    }
}