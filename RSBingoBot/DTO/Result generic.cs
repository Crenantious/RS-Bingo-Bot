// <copyright file="Result.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

using RSBingoBot.Exceptions;

internal class Result<T> : Result
{
    public T Value { get; }

    public Result(T value) : base()
    {
        Value = value;
    }
}