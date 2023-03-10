// <copyright file="RemoveTaskRestrictionsCSVOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.Interfaces;
using static RSBingo_Framework.DAL.DataFactory;

/// <inheritdoc/>
public class RemoveTaskRestrictionsCSVOperator : CSVOperator<RemoveTaskRestrictionCSVLine>
{
    protected override string WarningMessagesPrefix => "Unable to remove the following task restrictions (they likely does not exist): ";

    private IDataWorker dataWorker = CreateDataWorker();

    /// <inheritdoc/>
    protected override void OperateOnLine(RemoveTaskRestrictionCSVLine line)
    {
        try
        {
            dataWorker.Restrictions.Remove(
                dataWorker.Restrictions.GetByName(line.RestrictionName));
        }
        catch
        {
            AddWarning(line.RestrictionName, line);
        }
    }

    /// <inheritdoc/>
    protected override void OnPostOperating() =>
        dataWorker.SaveChanges();
}