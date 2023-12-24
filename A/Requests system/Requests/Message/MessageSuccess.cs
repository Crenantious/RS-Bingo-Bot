// <copyright file="MessageSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;
using FluentResults;

internal class MessageSuccess : Success
{
    public MessageSuccess(string prefix, DiscordMessage message, DiscordChannel channel) :
        base($"{prefix}. Id: {message.Id}, Channel name: {channel.Name}, Channel id: {channel.Id}")
    {

    }
}