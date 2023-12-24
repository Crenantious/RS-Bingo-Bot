// <copyright file="DeleteMessageSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

internal class DeleteMessageSuccess : MessageSuccess
{
    public DeleteMessageSuccess(Message message, DiscordChannel channel) :
        base("Successfully deleted a message.", message, channel)
    {

    }
}