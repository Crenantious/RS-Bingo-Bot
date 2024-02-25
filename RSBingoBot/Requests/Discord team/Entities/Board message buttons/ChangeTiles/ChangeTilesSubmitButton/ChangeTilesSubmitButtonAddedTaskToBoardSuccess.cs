// <copyright file="ChangeTilesSubmitButtonAddedTaskToBoardSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Framework.Models;

internal class ChangeTilesSubmitButtonAddedTaskToBoardSuccess : Success, IDiscordResponse
{
    private const string SuccessMessage = "Changed '{0}' to '{1}'.";

    public ChangeTilesSubmitButtonAddedTaskToBoardSuccess(BingoTask oldTask, BingoTask newTask) :
        base(SuccessMessage.FormatConst(oldTask.Name, newTask.Name))
    {

    }
}