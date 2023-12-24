// <copyright file="RemoveTaskRestrictionsCSVOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV;

using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingoBot.CSV.Lines;
using RSBingoBot.CSV.Operators.Warnings;

/// <inheritdoc/>
public class RemoveTaskRestrictionsCSVOperator : CSVOperator<RemoveTaskRestrictionCSVLine>
{
    public RemoveTaskRestrictionsCSVOperator(IDataWorker dataWorker)
        : base(dataWorker) { }

    /// <inheritdoc/>
    protected override async Task OperateOnLine(RemoveTaskRestrictionCSVLine line)
    {
        if (DataWorker.Restrictions.GetByName(line.RestrictionName.Value) is Restriction restriction)
        {
            DataWorker.Restrictions.Remove(restriction);
            return;
        }

        AddWarning(new TaskRestrictionDoesNotExistWarning(line.RestrictionName.ValueIndex, line.LineNumber));
    }

    /// <inheritdoc/>
    protected override void OnPostOperating() =>
        DataWorker.SaveChanges();
}