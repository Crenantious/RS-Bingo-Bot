// <copyright file="RemoveTaskRestrictionsCSVOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.CSV.Operators.Warnings;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using static RSBingo_Framework.DAL.DataFactory;

/// <inheritdoc/>
public class RemoveTaskRestrictionsCSVOperator : CSVOperator<RemoveTaskRestrictionCSVLine>
{
    private readonly IDataWorker dataWorker = CreateDataWorker();

    /// <inheritdoc/>
    protected override void OperateOnLine(RemoveTaskRestrictionCSVLine line)
    {
        if (dataWorker.Restrictions.GetByName(line.RestrictionName.Value) is Restriction restriction)
        {
            dataWorker.Restrictions.Remove(restriction);
            return;
        }

        AddWarning(new TaskRestrictionDoesNotExistWarning(line.RestrictionName.ValueIndex, line.LineNumber));
    }

    /// <inheritdoc/>
    protected override void OnPostOperating() =>
        dataWorker.SaveChanges();
}