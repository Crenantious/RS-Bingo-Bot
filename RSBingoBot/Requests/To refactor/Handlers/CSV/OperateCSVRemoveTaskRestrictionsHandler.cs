// <copyright file="OperateCSVRemoveTaskRestrictionsHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.Models;

/// <summary>
/// Request for removing <see cref="Restriction"/>s via a csv file.
/// </summary>
internal class OperateCSVRemoveTaskRestrictionsHandler : OperateCSVHandlerBase<RemoveTaskRestrictionCSVLine>
{
    /// <inheritdoc/>
    protected override string SuccessResponse => "The restrictions were successfully removed.";

    /// <inheritdoc/>
    private protected override IEnumerable<string> Operate()
    {
        RemoveTaskRestrictionsCSVOperator op = new(DataWorker);
        op.Operate(Data);
        return op.GetWarningMessages();
    }
}