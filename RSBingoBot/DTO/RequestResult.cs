// <copyright file="RequestResult.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

using RSBingoBot.Exceptions;

internal class RequestResult
{
    public bool IsCompleted { get; }
    public bool IsFaulted { get; }
    public IEnumerable<string> Responses { get; }
    private RequestException? exception { get; }

    public IEnumerable<string> Errors =>
        exception is null ?
        Enumerable.Empty<string>() :
        exception.Errors;

    public RequestResult(bool isCompleted)
    {
        Responses = Enumerable.Empty<string>();
        IsCompleted = isCompleted;
        IsFaulted = !isCompleted;
    }

    public RequestResult(IEnumerable<string> responses, bool isCompleted)
    {
        Responses = responses;
        IsCompleted = isCompleted;
        IsFaulted = !isCompleted;
    }

    public RequestResult(RequestException exception)
    {
        this.exception = exception;
        Responses = exception.Errors;
        IsCompleted = false;
        IsFaulted = true;
    }
}