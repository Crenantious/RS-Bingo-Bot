// <copyright file="AddTaskRestrictionsCSVOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.CSV.Operators.Warnings;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using static RSBingo_Framework.DAL.DataFactory;

/// <inheritdoc/>
public class AddTaskRestrictionsCSVOperator : CSVOperator<AddTaskRestrictionCSVLine>
{
    private readonly IDataWorker dataWorker = CreateDataWorker();
    private readonly HashSet<string> existingRestrictionNames = new();

    /// <inheritdoc/>
    protected override void OnPreOperating()
    {
        foreach (Restriction restriction in dataWorker.Restrictions.GetAll())
        {
            existingRestrictionNames.Add(restriction.Name);
        }
    }

    /// <inheritdoc/>
    protected override void OperateOnLine(AddTaskRestrictionCSVLine line)
    {
        if (existingRestrictionNames.Contains(line.RestrictionName.Value))
        {
            AddWarning(new TaskRestrictionAlreadyExistsWarning(line.RestrictionName.ValueIndex, line.LineNumber));
            return;
        }

        Restriction restriction = dataWorker.Restrictions.Create(line.RestrictionName.Value, line.RestrictionDescription.Value);
        existingRestrictionNames.Add(line.RestrictionName.Value);
    }

    /// <inheritdoc/>
    protected override void OnPostOperating() =>
        dataWorker.SaveChanges();
}