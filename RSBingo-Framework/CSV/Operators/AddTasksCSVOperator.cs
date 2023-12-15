// <copyright file="AddTasksCSVOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using FluentResults;
using RSBingo_Common;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.CSV.Operators.Warnings;
using RSBingo_Framework.Interfaces;
using static RSBingo_Common.Paths;

/// <inheritdoc/>
public class AddTasksCSVOperator : CSVOperator<AddTasksCSVLine>
{
    IWebServices webServices;

    public AddTasksCSVOperator(IDataWorker dataWorker) : base(dataWorker)
    {
        webServices = (IWebServices)General.DI.GetService(typeof(IWebServices));
    }

    /// <inheritdoc/>
    protected override void OperateOnLine(AddTasksCSVLine line)
    {
        string imagePath = GetTaskImagePath(line.TaskName.Value);

        // The image will be used elsewhere.
        Result result = await webServices.DownloadFile(line.TaskUrl.Value, imagePath);
        if (result.IsFaulted)
        {
            AddWarning(new UnableToDownloadImageWarning(result.Error, line.TaskUrl.ValueIndex, line.LineNumber));
            return;
        }

        if (General.ValidateImage(imagePath) is false)
        {
            AddWarning(new InvalidImageWarning(line.TaskUrl.ValueIndex, line.LineNumber));
            return;
        }

        DataWorker.BingoTasks.CreateMany(line.TaskName.Value,
            line.TaskDifficulty.Value,
            line.AmountOfTasks.Value);
    }

    /// <inheritdoc/>
    protected override void OnPostOperating() =>
        DataWorker.SaveChanges();
}