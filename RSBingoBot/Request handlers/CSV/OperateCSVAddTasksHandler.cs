﻿// <copyright file="OperateCSVAddTasksHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.Models;

/// <summary>
/// Request for adding <see cref="BingoTask"/>s via a csv file.
/// </summary>
internal class OperateCSVAddTasksHandler : OperateCSVHandlerBase<AddTasksCSVLine>
{
    /// <inheritdoc/>
    protected override string SuccessResponse => "The tasks were successfully added.";

    /// <inheritdoc/>
    private protected override IEnumerable<string> Operate()
    {
        AddTasksCSVOperator op = new(DataWorker);
        op.Operate(Data);
        return op.GetWarningMessages();
    }
}