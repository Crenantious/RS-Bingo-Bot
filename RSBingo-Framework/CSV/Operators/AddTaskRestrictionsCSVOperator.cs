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
    private HashSet<string> existingRestrictionNames = null!;

    public AddTaskRestrictionsCSVOperator(IDataWorker dataWorker)
        : base(dataWorker) { }

    /// <inheritdoc/>
    protected override void OnPreOperating()
    {
        existingRestrictionNames = DataWorker.Restrictions.GetAll().Select(r => r.Name).ToHashSet();
    }

    /// <inheritdoc/>
    protected override void OperateOnLine(AddTaskRestrictionCSVLine line)
    {
        if (existingRestrictionNames.Contains(line.RestrictionName.Value))
        {
            AddWarning(new TaskRestrictionAlreadyExistsWarning(line.RestrictionName.ValueIndex, line.LineNumber));
            return;
        }

        Restriction restriction = DataWorker.Restrictions.Create(line.RestrictionName.Value, line.RestrictionDescription.Value);
        existingRestrictionNames.Add(line.RestrictionName.Value);
    }

    /// <inheritdoc/>
    protected override void OnPostOperating() =>
        DataWorker.SaveChanges();
}