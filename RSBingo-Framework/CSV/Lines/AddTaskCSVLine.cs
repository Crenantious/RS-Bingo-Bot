// <copyright file="AddTaskCSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using RSBingo_Framework.Exceptions;

namespace RSBingo_Framework.CSV.Lines;

public class AddTaskCSVLine : AddOrRemoveTaskCSVLine
{
    public string TaskImageUrl { get; private set; } = String.Empty;

    private CSVValueGeneric<string> taskUrlValue = new("Task URL", 3);

    public AddTaskCSVLine(int lineNumber, string[] values) : base(lineNumber, values) {  }

    protected override void Parse(string[] values)
    {
        base.Parse(values);
        TaskImageUrl = taskUrlValue.Parse(values);
    }
}