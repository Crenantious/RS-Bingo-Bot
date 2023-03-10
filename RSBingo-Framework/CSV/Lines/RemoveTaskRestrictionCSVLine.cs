// <copyright file="RemoveTaskRestrictionCSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV.Lines;

public class RemoveTaskRestrictionCSVLine : CSVLine
{
    private CSVValueGeneric<string> restrictionNameValue = new("Task restriction name", 0);

    public string RestrictionName { get; private set; } = String.Empty;

    protected override int NumberOfValues => 1;

    public RemoveTaskRestrictionCSVLine(int lineNumber, string[] values) : base(lineNumber, values) { }

    protected override void Parse(string[] values) =>
        RestrictionName = restrictionNameValue.Parse(values);
}