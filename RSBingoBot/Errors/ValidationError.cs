// <copyright file="ValidationError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Errors;

using FluentResults;

internal class ValidationError : Error
{
    public ValidationError(string message) : base(message)
    {
    }
}