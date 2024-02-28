// <copyright file="SubmitDropSubmitButtonSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Framework.Models;

internal class SubmitDropSubmitButtonSuccess : Success, IDiscordResponse
{
    private const string SuccessMessage = "Evidence submitted for {0}.";

    public SubmitDropSubmitButtonSuccess(Tile tile) : base(SuccessMessage.FormatConst(tile.Task.Name))
    {

    }
}