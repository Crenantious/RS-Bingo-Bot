// <copyright file="AddTaskRestrictionsCSVOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using static RSBingo_Framework.DAL.DataFactory;


/// <inheritdoc/>
public class AddTaskRestrictionsCSVOperator : CSVOperator<AddTaskRestrictionCSVLine>
{
    protected override string ErrorMessagePrefix => "Restrictions with the following names already exist, so they were ignored: ";

    private IDataWorker dataWorker = CreateDataWorker();
    private readonly HashSet<string> restrictionNames = new();

    protected override void OnPreOperating()
    {
        foreach (Restriction restriction in dataWorker.Restrictions.GetAll())
        {
            restrictionNames.Add(restriction.Name);
        }
    }

    protected override void OperateOnLine(AddTaskRestrictionCSVLine line)
    {
        if (restrictionNames.Contains(line.RestrictionName))
        {
            AddWarning(line, line.RestrictionName);
        }
        else
        {
            Restriction restriction = dataWorker.Restrictions.Create(line.RestrictionName, line.RestrictionDescription);
            restrictionNames.Add(line.RestrictionName);
        }
    }

    protected override void OnPostOperating() =>
        dataWorker.SaveChanges();
}