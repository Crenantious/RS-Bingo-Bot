// <copyright file="RequestResult generic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

internal class RequestResult<T> : RequestResult
{
    public T Value { get; }

    public RequestResult(T value) : base()
    {
        Value = value;
    }
}