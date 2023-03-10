// <copyright file="RemoveTasksCSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV.Lines;

public class RemoveTasksCSVLine : AddOrRemoveTasksCSVLine
{
    public RemoveTasksCSVLine(int lineNumber, string[] values) : base(lineNumber, values) { }

    protected override void Parse(string[] values) =>
        base.Parse(values);
}