// <copyright file="EvidenceMissingError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using RSBingo_Framework.Models;
using System.Collections.Generic;

internal class EvidenceMissingError : IError
{
    private const string message = "Evidence can no longer be found for the tile {0}";

    public Dictionary<string, object> Metadata => new();

    public List<IError> Reasons { get; } = new();

    public string Message { get; }

    public EvidenceMissingError(Tile tile) =>
        Message = message.FormatConst(tile.Task.Name);
}