// <copyright file="AddTaskRestrictionsCSVOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV;

using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingoBot.CSV.Lines;
using RSBingoBot.CSV.Operators.Warnings;

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
    protected override async Task OperateOnLine(AddTaskRestrictionCSVLine line)
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