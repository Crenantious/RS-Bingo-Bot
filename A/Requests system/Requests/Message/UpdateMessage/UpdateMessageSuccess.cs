// <copyright file="UpdateMessageSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests;

internal class UpdateMessageSuccess : MessageSuccess
{
    public UpdateMessageSuccess(IMessage message) :
        base("Updated a message.", message.DiscordMessage)
    {

    }
}