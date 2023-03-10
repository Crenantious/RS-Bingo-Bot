// <copyright file="AddTaskCSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV.Lines;

using RSBingo_Framework.Exceptions;

public class AddTaskCSVLine : AddOrRemoveTasksCSVLine
{
    private CSVValueGeneric<string> taskUrlValue = new("Task URL", 3);

    public string TaskImageUrl { get; private set; } = String.Empty;

    public AddTaskCSVLine(int lineNumber, string[] values) : base(lineNumber, values) {  }

    protected override void Parse(string[] values)
    {
        base.Parse(values);
        TaskImageUrl = taskUrlValue.Parse(values);
    }
}