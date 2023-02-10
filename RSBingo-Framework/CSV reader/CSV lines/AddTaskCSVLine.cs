// <copyright file="AddTaskCSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using RSBingo_Framework.Exceptions;

namespace RSBingo_Framework.CSV_reader.CSV_lines;

internal class AddTaskCSVLine : AddOrRemoveTaskCSVLine
{
    public string TaskImageUrl { get; private set; } = String.Empty;

    private CSVValueGeneric<string> taskUrlValue = new("Task URL", 3);

    public override void Parse(string[] values)
    {
        base.Parse(values);
        TaskImageUrl = taskUrlValue.Parse(values);
    }
}