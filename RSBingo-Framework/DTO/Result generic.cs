// <copyright file="Result.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DTO;

public class Result<T> : Result
{
    public T Value { get; }

    public Result(T value) : base()
    {
        Value = value;
    }
}