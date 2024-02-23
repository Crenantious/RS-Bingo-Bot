// <copyright file="SelectComponentBackButtonFirstItemError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;

internal class SelectComponentBackButtonFirstItemError : Error, IDiscordResponse
{
    private const string ErrorMessage = "Can't go back any further.";

    public SelectComponentBackButtonFirstItemError() : base(ErrorMessage)
    {

    }
}