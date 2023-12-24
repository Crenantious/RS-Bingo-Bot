// <copyright file="AddOrRemoveTaskCSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV.Lines;

using static RSBingo_Framework.Records.BingoTaskRecord;

public abstract class AddOrRemoveTasksCSVLine : CSVLine
{
    // This is an arbitrary value to not crash the bot and to not eat memory in the DB.
    // Currently, there is no need to use anywhere near this number.
    // This does not limit the user from creating multiple lines that add the max number of tasks,
    // rather it prevents them from inadvertently entering a large number.
    public const int MaxNumberOfTasks = 10;
    public const int MinNumberOfTasks = 1;

    public CSVValueGeneric<string> TaskName { get; } = new("Task name", 0);
    public CSVValueEnum<Difficulty> TaskDifficulty { get; } = new("Task difficulty", 1);
    public CSVValueComparable<int> AmountOfTasks { get; } = new("Amount of tasks", 2, MinNumberOfTasks, MaxNumberOfTasks);

    public AddOrRemoveTasksCSVLine(int lineNumber, string[] values) : base(lineNumber, values) { }

    protected override List<ICSVValue> GetValues() =>
        new List<ICSVValue> { TaskName, TaskDifficulty, AmountOfTasks };
}