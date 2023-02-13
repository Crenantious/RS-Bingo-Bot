// <copyright file="AddOrRemoveTaskCSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using RSBingo_Framework.Exceptions;
using static RSBingo_Framework.Records.BingoTaskRecord;

namespace RSBingo_Framework.CSV.Lines;

public class AddOrRemoveTaskCSVLine : CSVLine
{
    // This is an arbitrary value to not crash the bot and to not eat memory in the DB.
    // Currently, there is no need to use anywhere near this number.
    private const int MaxNumberOfTasks = 10;

    public string TaskName { get; private set; } = String.Empty;    
    public Difficulty TaskDifficulty { get; private set; }
    public int AmountOfTasks { get; private set; }

    private CSVValueGeneric<string> nameValue = new("Task name", 0);
    private CSVValueEnum<Difficulty> difficultyValue = new("Task difficulty", 1, false);
    private CSVValueCompareable<int> amountOfTasksValue = new("Amount of tasks", 2, 1, MaxNumberOfTasks);

    public AddOrRemoveTaskCSVLine(int lineNumber, string[] values) : base(lineNumber, values) { }

    protected override void Parse(string[] values)
    {
        TaskName = nameValue.Parse(values);
        TaskDifficulty = difficultyValue.Parse(values);
        AmountOfTasks = amountOfTasksValue.Parse(values);
    }
}