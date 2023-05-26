// <copyright file="RequestException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Exceptions;

using RSBingo_Framework.Exceptions;

internal class RequestException : Exception
{
    public IEnumerable<string> Errors { get; set; }

    public RequestException(string message) : base(message)
    {
        Errors = new List<string> { message };
    }

    public RequestException(IEnumerable<string> errors)
    {
        Errors = errors;
    }
}