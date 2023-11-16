// <copyright file="HandlerSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

/// <inheritdoc/>
internal abstract class HandlerSuccess : Success, ISuccess
{
    public HandlerSuccess(string message) : base(message)
    {

    }
}