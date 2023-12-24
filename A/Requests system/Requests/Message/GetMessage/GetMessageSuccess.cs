// <copyright file="GetMessageSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

internal class GetMessageSuccess : MessageSuccess
{
    public GetMessageSuccess(Message message, DiscordChannel channel) :
        base("Successfully retrieved message", message, channel)
    {
    }
}