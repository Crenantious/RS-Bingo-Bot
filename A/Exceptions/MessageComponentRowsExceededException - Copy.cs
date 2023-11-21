// <copyright file="MessageComponentRowsExceededException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Exceptions;

using RSBingo_Common;

internal class MessageComponentRowsExceededException : Exception
{
    private const string message = "Cannot add more component rows; max {0}.";

    public MessageComponentRowsExceededException(int max) :
        base(message.FormatConst(max))
    {

    }
}