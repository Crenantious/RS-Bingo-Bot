// <copyright file="GetMessageSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;

internal class GetMessageSuccess : MessageSuccess
{
    public GetMessageSuccess(Message message) :
        base("retrieved a message", message.DiscordMessage)
    {

    }
}