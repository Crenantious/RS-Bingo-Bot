// <copyright file="SendMessageSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

internal class SendMessageSuccess : MessageSuccess
{
    public SendMessageSuccess(Message message, DiscordChannel channel) :
        base("Successfully sent a message.", message, channel)
    {

    }
}