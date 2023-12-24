// <copyright file="AddTaskCSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV.Lines;

public class AddTasksCSVLine : AddOrRemoveTasksCSVLine
{
    public CSVValueGeneric<string> TaskUrl { get; } = new("Task URL", 3);

    public AddTasksCSVLine(int lineNumber, string[] values) : base(lineNumber, values) { }

    protected override List<ICSVValue> GetValues() =>
        new List<ICSVValue>(base.GetValues()) { TaskUrl };
}