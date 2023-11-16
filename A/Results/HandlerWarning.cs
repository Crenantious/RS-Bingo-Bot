// <copyright file="HandlerWarning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;

/// <inheritdoc/>
public abstract class HandlerWarning : Success, IWarning
{
    public HandlerWarning(string message) : base(message)
    {

    }
}