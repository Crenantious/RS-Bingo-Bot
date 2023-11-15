// <copyright file="ProcessError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using System.Collections.Generic;

internal record ProcessError(string Message) : IError
{
    public Dictionary<string, object> Metadata => new();

    public List<IError> Reasons { get; } = new();
}