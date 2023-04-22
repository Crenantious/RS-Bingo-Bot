// <copyright file="AddOrRemoveTaskRestrictionCSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV.Lines;

public class AddOrRemoveTaskRestrictionCSVLine : CSVLine
{
    public CSVValueGeneric<string> RestrictionName { get; } = new("Task restriction name", 0);

    public AddOrRemoveTaskRestrictionCSVLine(int lineNumber, string[] values) : base(lineNumber, values) { }

    protected override List<ICSVValue> GetValues() =>
        new List<ICSVValue>() { RestrictionName };
}