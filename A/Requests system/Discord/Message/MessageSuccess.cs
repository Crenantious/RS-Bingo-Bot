// <copyright file="MessageSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DSharpPlus.Entities;
using FluentResults;

internal class MessageSuccess : Success
{
    public MessageSuccess(string prefix, Message message, DiscordChannel channel) :
        base(prefix + MessageRequestInformation.Get(message, channel))
    {

    }
}