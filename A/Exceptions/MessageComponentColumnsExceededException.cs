// <copyright file="MessageComponentColumnsExceededException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Exceptions;

using RSBingo_Common;

internal class MessageComponentColumnsExceededException : Exception
{
    private const string message = "Was given {0} components in row; max {1}.";

    public MessageComponentColumnsExceededException(int max) :
        base(message.FormatConst(max))
    {

    }
}