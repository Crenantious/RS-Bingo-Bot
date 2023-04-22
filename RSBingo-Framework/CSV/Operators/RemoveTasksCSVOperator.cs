// <copyright file="RemoveTasksCSVOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.CSV.Operators.Warnings;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using static RSBingo_Framework.DAL.DataFactory;
    
/// <inheritdoc/>
public class RemoveTasksCSVOperator : CSVOperator<RemoveTasksCSVLine>
{
    private readonly IDataWorker dataWorker = CreateDataWorker();

    /// <inheritdoc/>
    protected override void OperateOnLine(RemoveTasksCSVLine line)
    {
        IEnumerable<BingoTask> tasks = dataWorker.BingoTasks.GetByNameAndDifficulty(line.TaskName.Value, line.TaskDifficulty.Value)
                                           .Take(line.AmountOfTasks.Value);
        int tasksCount = tasks.Count();

        if (tasksCount > 0)
        {
            if (tasksCount < line.AmountOfTasks.Value)
            {
                AddWarning(new NotEnoughTasksToDeleteWarning(line.TaskName.ValueIndex, line.LineNumber, tasksCount));
            }

            dataWorker.BingoTasks.RemoveRange(tasks);
            return;
        }

        AddWarning(new TaskDoesNotExistWarning(line.TaskName.ValueIndex, line.LineNumber));
    }

    /// <inheritdoc/>
    protected override void OnPostOperating() =>
        dataWorker.SaveChanges();
}