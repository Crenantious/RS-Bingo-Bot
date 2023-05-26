// <copyright file="Result.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

internal readonly struct Result<T>
{
    public readonly T Value;
    public readonly bool IsSuccess;
    public readonly bool IsFaulted;
    private readonly Exception? exception;

    public string ErrorMessage => exception.Message;

    public Result(T value)
    {
        Value = value;
        IsSuccess = true;
        IsFaulted = false;
    }

    public Result(Exception exception)
    {
        this.exception = exception;
        IsSuccess = false;
        IsFaulted = true;
    }
}