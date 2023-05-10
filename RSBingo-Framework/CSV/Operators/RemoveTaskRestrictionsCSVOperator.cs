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
    public RemoveTaskRestrictionsCSVOperator(IDataWorker dataWorker)
        : base(dataWorker) { }

    /// <inheritdoc/>
    protected override void OperateOnLine(RemoveTaskRestrictionCSVLine line)
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