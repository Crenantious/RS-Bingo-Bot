// <copyright file="DeleteMessageSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

internal class DeleteMessageSuccess : MessageSuccess
{
    public DeleteMessageSuccess(DiscordMessage message) :
        base("deleted a message", message)
    {

    }
}