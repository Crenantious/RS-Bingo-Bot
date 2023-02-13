// <copyright file="RemoveTasksCSVOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.Interfaces;
using static RSBingo_Framework.DAL.DataFactory;
    
/// <inheritdoc/>
public class RemoveTasksCSVOperator : CSVOperator<RemoveTaskCSVLine>
{
    private IDataWorker dataWorker = CreateDataWorker();

    protected override void OperateOnLine(RemoveTaskCSVLine line)
    {
        dataWorker.BingoTasks.DeleteMany(
            dataWorker.BingoTasks.GetByNameAndDifficulty(line.TaskName, line.TaskDifficulty)
            .Take(line.AmountOfTasks));
    }

    protected override void OnPostOperating() =>
        dataWorker.SaveChanges();
}