// <copyright file="EvidenceMissingError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Framework.Models;

internal class EvidenceMissingError : Error, IDiscordResponse
{
    private const string ErrorMessage = "Evidence can no longer be found for the tile {0}.";

    public EvidenceMissingError(Tile tile) : base(ErrorMessage.FormatConst(tile.Task.Name))
    {

    }
}