// <copyright file="RemoveTaskRestrictionCSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV.Lines;

internal class RemoveTaskRestrictionCSVLine : CSVLine
{
    public string RestrictionName { get; private set; } = String.Empty;    

    private CSVValueGeneric<string> restrictionNameValue = new("Task restriction name", 0);

    public RemoveTaskRestrictionCSVLine(int lineNumber, string[] values) : base(lineNumber, values) { }

    protected override void Parse(string[] values) =>
        RestrictionName = restrictionNameValue.Parse(values);
}