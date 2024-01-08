﻿// <copyright file="ChangeTilesSubmitButtonAddedTaskToBoardSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Framework.Models;

internal class ChangeTilesSubmitButtonAddedTaskToBoardSuccess : Success, IDiscordResponse
{
    private const string SuccessMessage = "Added '{0}' to the board.";

    public ChangeTilesSubmitButtonAddedTaskToBoardSuccess(BingoTask task) : base(SuccessMessage.FormatConst(task.Name))
    {

    }
}