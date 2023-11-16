// <copyright file="HandlerSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;

/// <inheritdoc/>
public abstract class HandlerSuccess : Success, ISuccess
{
    public HandlerSuccess(string message) : base(message)
    {

    }
}