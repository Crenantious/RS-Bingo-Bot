// <copyright file="DiscordError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;

internal class DiscordError : Error, IDiscordResponse
{
    public const string ErrorMessage = "A Discord error occurred.";

    public DiscordError() : base(ErrorMessage)
    {

    }
}