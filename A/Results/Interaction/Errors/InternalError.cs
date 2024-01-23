// <copyright file="InternalError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;

internal class InternalError : Error, IDiscordResponse
{
    public const string ErrorMessage = "An internal error occurred.";

    public InternalError() : base(ErrorMessage)
    {

    }
}