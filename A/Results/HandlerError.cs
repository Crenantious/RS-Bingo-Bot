// <copyright file="HandlerError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;

public abstract class HandlerError : Error, IError
{
    public HandlerError(string message) : base(message)
    {

    }
}