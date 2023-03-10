// <copyright file="RemoveTaskCSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV.Lines;

public class RemoveTaskCSVLine : AddOrRemoveTasksCSVLine
{
    public RemoveTaskCSVLine(int lineNumber, string[] values) : base(lineNumber, values) { }

    protected override void Parse(string[] values) =>
        base.Parse(values);
}