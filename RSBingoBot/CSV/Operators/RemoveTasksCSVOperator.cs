// <copyright file="RemoveTasksCSVOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV;

using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingoBot.CSV.Lines;
using RSBingoBot.CSV.Operators.Warnings;

/// <inheritdoc/>
public class RemoveTasksCSVOperator : CSVOperator<RemoveTasksCSVLine>
{
    public RemoveTasksCSVOperator(IDataWorker dataWorker)
        : base(dataWorker) { }

    /// <inheritdoc/>
    protected override async Task OperateOnLine(RemoveTasksCSVLine line)
    {
        IEnumerable<BingoTask> tasks = DataWorker.BingoTasks.GetByNameAndDifficulty(line.TaskName.Value, line.TaskDifficulty.Value)
                                           .Take(line.AmountOfTasks.Value);
        int tasksCount = tasks.Count();

        if (tasksCount <= 0)
        {
            AddWarning(new TaskDoesNotExistWarning(line.TaskName.ValueIndex, line.LineNumber));
            return;
        }

        DataWorker.BingoTasks.RemoveRange(tasks);
        return;
    }

    /// <inheritdoc/>
    protected override void OnPostOperating() =>
        DataWorker.SaveChanges();
}