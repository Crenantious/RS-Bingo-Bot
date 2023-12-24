// <copyright file="AddTasksCSVOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV;

using DiscordLibrary.DiscordServices;
using FluentResults;
using RSBingo_Common;
using RSBingo_Framework.Interfaces;
using RSBingoBot.CSV.Lines;
using RSBingoBot.CSV.Operators.Warnings;
using static RSBingo_Common.Paths;

/// <inheritdoc/>
public class AddTasksCSVOperator : CSVOperator<AddTasksCSVLine>
{
    private IWebServices webServices;

    public AddTasksCSVOperator(IDataWorker dataWorker) : base(dataWorker)
    {
        webServices = (IWebServices)General.DI.GetService(typeof(IWebServices));
    }

    /// <inheritdoc/>
    protected override async Task OperateOnLine(AddTasksCSVLine line)
    {
        string imagePath = GetTaskImagePath(line.TaskName.Value);

        // The image will be used elsewhere.
        Result result = await webServices.DownloadFile(line.TaskUrl.Value, imagePath);
        if (result.IsFailed)
        {
            AddWarning(new UnableToDownloadImageWarning(result.Errors, line.TaskUrl.ValueIndex, line.LineNumber));
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