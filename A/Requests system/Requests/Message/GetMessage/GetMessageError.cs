// <copyright file="GetMessageError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

internal class GetMessageError : MessageError
{
    private const string ErrorPrefix = "unable to retrieve message";
    private const string ErrorReason = "the message does not exist";

    public GetMessageError(ulong id, DiscordChannel channel) :
        base(ErrorPrefix, ErrorReason, id, channel)
    {

    }
}