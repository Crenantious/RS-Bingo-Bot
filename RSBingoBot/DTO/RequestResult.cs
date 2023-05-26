// <copyright file="RequestResult.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

using RSBingoBot.Exceptions;

internal class RequestResult
{
    public bool IsSuccess { get; }
    public bool IsFaulted { get; }
    public RequestException? exception { get; }

    public IEnumerable<string> Errors =>
        exception is null ?
        Enumerable.Empty<string>() :
        exception.Errors;

    public RequestResult()
    {
        IsSuccess = true;
        IsFaulted = false;
    }

    public RequestResult(RequestException exception)
    {
        this.exception = exception;
        IsSuccess = false;
        IsFaulted = true;
    }
}