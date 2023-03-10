// <copyright file="AddTasksCSVOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using System.Net;
using Microsoft.EntityFrameworkCore;
using RSBingo_Framework.Exceptions;
using SixLabors.ImageSharp;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.Interfaces;
using static RSBingo_Common.General;
using static RSBingo_Framework.DAL.DataFactory;

/// <inheritdoc/>
public class AddTasksCSVOperator : CSVOperator<AddTaskCSVLine>
{
    private IDataWorker dataWorker = CreateDataWorker();

    public AddTasksCSVOperator()
    {
        // Reset auto increment just in case it overflows
        // TODO: this should not be done here; move it to somewhere appropriate.
        dataWorker.Context.Database.ExecuteSqlRaw("ALTER TABLE task AUTO_INCREMENT = 1");
    }

    /// <inheritdoc/>
    protected override void OperateOnLine(AddTaskCSVLine line)
    {
        // The image will be used elsewhere.
        if (DownloadTaskImage(line) is string warning)
        {
            AddWarning(warning, line);
            return;
        }

        dataWorker.BingoTasks.CreateMany(line.TaskName,
            line.TaskDifficulty,
            line.AmountOfTasks);
    }

    /// <inheritdoc/>
    protected override void OnPostOperating() =>
        dataWorker.SaveChanges();

    private string DownloadTaskImage(AddTaskCSVLine line)
    {
        try
        {
            WebClient client = new();
            client.DownloadFile(line.TaskName, GetTaskImagePath(line.TaskName));
        }
        catch (Exception e)
        {
            string errorMessage = e switch
            {
                NotSupportedException => "Image format is unsupported",
                WebException => "Image format is unsupported",
                UnknownImageFormatException => "Unknown image format",
                InvalidImageContentException => "Invalid image content",
                _ => string.Empty
            };

            if (errorMessage == string.Empty) { throw; }
            return errorMessage;
        }

        return string.Empty;
    }
}