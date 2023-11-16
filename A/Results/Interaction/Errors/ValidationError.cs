// <copyright file="ValidationError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;

public class ValidationError : Error
{
    public ValidationError(string message) : base(message)
    {

    }
}