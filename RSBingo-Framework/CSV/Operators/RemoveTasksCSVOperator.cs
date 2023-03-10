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
    protected override string WarningMessagesPrefix => "Unable to remove the following tasks (they likely does not exist): ";

    private IDataWorker dataWorker = CreateDataWorker();

    /// <inheritdoc/>
    protected override void OperateOnLine(RemoveTaskCSVLine line)
    {
        try
        {
            dataWorker.BingoTasks.DeleteMany(
                dataWorker.BingoTasks.GetByNameAndDifficulty(line.TaskName, line.TaskDifficulty)
                .Take(line.AmountOfTasks));
        }
        catch
        {
            AddWarning(line.TaskName, line);
        }
    }

    /// <inheritdoc/>
    protected override void OnPostOperating() =>
        dataWorker.SaveChanges();
}