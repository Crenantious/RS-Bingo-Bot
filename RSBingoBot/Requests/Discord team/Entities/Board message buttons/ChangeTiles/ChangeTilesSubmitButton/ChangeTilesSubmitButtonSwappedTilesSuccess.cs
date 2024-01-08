// <copyright file="ChangeTilesSubmitButtonSwappedTilesSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Framework.Models;

internal class ChangeTilesSubmitButtonSwappedTilesSuccess : Success, IDiscordResponse
{
    private const string SuccessMessage = "Swapped '{0}' and '{1}'.";

    public ChangeTilesSubmitButtonSwappedTilesSuccess(Tile tile1, Tile tile2) :
        base(SuccessMessage.FormatConst(tile1.Task.Name, tile2.Task.Name))
    {

    }
}