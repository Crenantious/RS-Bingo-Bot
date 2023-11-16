// <copyright file="HandlerError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

internal abstract class HandlerError : Error, IError
{
    public HandlerError(string message) : base(message)
    {

    }
}