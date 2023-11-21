// <copyright file="MessageAddSelectComponentException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Exceptions;

using DiscordLibrary.DiscordComponents;

internal class MessageAddSelectComponentException : Exception
{
    private const string message = $"Cannot have {nameof(SelectComponent)} in the same row as other components.";

    public MessageAddSelectComponentException() : base(message)
    {

    }
}