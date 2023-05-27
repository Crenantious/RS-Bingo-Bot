// <copyright file="Result.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

internal class Result
{
    public bool IsCompleted { get; }
    public bool IsFaulted { get; }
    public string Error => exception is null ? string.Empty : exception.Message;
    private Exception? exception { get; }

    public Result()
    {
        IsCompleted = true;
        IsFaulted = false;
    }

    public Result(Exception exception)
    {
        this.exception = exception;
        IsCompleted = false;
        IsFaulted = true;
    }
}