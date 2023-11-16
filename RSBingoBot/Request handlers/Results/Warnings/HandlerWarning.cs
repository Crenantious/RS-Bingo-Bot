// <copyright file="HandlerWarning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

/// <inheritdoc/>
internal abstract class HandlerWarning : Success, IWarning
{
    public HandlerWarning(string message) : base(message)
    {

    }
}