// <copyright file="SelectComponentNoOptionsException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Exceptions;

internal class SelectComponentNoOptionsException : Exception
{
    private const string message = "A select component must contain at least one option when being sent with a message..";

    public SelectComponentNoOptionsException()
    {

    }
}