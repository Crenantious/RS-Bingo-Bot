// <copyright file="OperateCSVAddTaskRestrictionsHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;

internal class OperateCSVAddTaskRestrictionsHandler : OperateCSVHandlerBase<AddTaskRestrictionCSVLine>
{
    /// <inheritdoc/>
    protected override string ProcessSuccessResponse => "The restrictions were successfully added to the database";

    /// <inheritdoc/>
    private protected override IEnumerable<string> Operate()
    {
        AddTaskRestrictionsCSVOperator op = new(DataWorker);
        op.Operate(Data);
        return op.GetWarningMessages();
    }
}