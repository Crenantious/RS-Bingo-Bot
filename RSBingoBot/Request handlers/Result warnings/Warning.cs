// <copyright file="Warning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using System.Collections.Generic;

internal record Warning(string Message) : ISuccess
{
    public Dictionary<string, object> Metadata => new();
}