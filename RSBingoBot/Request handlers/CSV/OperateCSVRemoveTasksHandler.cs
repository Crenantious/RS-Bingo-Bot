// <copyright file="OperateCSVRemoveTasksHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.Models;

/// <summary>
/// Request for removing <see cref="BingoTask"/>s via a csv file.
/// </summary>
internal class OperateCSVRemoveTasksHandler : OperateCSVHandlerBase<RemoveTasksCSVLine>
{
    /// <inheritdoc/>
    protected override string SuccessResponse => "The tasks were successfully removed.";

    /// <inheritdoc/>
    private protected override IEnumerable<string> Operate()
    {
        RemoveTasksCSVOperator op = new(DataWorker);
        op.Operate(Data);
        return op.GetWarningMessages();
    }
}