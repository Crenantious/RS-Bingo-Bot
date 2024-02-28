// <copyright file="DeleteMessageError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

internal class DeleteMessageError : MessageError
{
    private const string ErrorPrefix = "unable to delete message";
    private const string ErrorReason = "the message does not exist";

    public DeleteMessageError(DiscordMessage message) :
        base(ErrorPrefix, ErrorReason, message.Id, message.Channel)
    {

    }
}