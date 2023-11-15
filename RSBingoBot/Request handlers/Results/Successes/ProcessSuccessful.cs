// <copyright file="ProcessSuccessful.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using System.Collections.Generic;

internal record ProcessSuccessful(string Message) : ISuccess
{
    public Dictionary<string, object> Metadata => new();
}