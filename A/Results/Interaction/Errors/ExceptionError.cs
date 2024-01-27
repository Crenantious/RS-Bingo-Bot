// <copyright file="ExceptionError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;

internal class ExceptionError : Error
{
    public ExceptionError(Exception exception) : base(exception.Message)
    {

    }
}