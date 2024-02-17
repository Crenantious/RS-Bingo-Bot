// <copyright file="MessageComponentRowEmptyException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Exceptions;

internal class MessageComponentRowEmptyException : Exception
{
    private const string message = "A component row cannot be empty.";

    public MessageComponentRowEmptyException()
    {

    }
}