// <copyright file="AddTasksCSVOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using System.Net;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Imaging;
using RSBingo_Framework.CSV.Operators.Warnings;
using static RSBingo_Common.General;
using static RSBingo_Framework.DAL.DataFactory;

/// <inheritdoc/>
public class AddTasksCSVOperator : CSVOperator<AddTasksCSVLine>
{
    private readonly IDataWorker dataWorker = CreateDataWorker();

    public AddTasksCSVOperator()
    {
        // Reset auto increment just in case it overflows
        // TODO: this should not be done here; move it to somewhere appropriate.
        //dataWorker.Context.Database.ExecuteSqlRaw("ALTER TABLE task AUTO_INCREMENT = 1");
    }

    /// <inheritdoc/>
    protected override void OperateOnLine(AddTasksCSVLine line)
    {
        string imagePath = GetTaskImagePath(line.TaskName.Value);

        // The image will be used elsewhere.
        if (DownloadTaskImage(line, imagePath) is Warning warning)
        {
            AddWarning(warning);
            return;
        }

        if (ValidateImage.ValidatePath(imagePath) is false)
        {
            AddWarning(new InvalidImageWarning(line.TaskUrl.ValueIndex, line.LineNumber));
            return;
        }

        dataWorker.BingoTasks.CreateMany(line.TaskName.Value,
            line.TaskDifficulty.Value,
            line.AmountOfTasks.Value);
    }

    /// <inheritdoc/>
    protected override void OnPostOperating() =>
        dataWorker.SaveChanges();

    private Warning? DownloadTaskImage(AddTasksCSVLine line, string imagePath)
    {
        try
        {
            WebClient client = new();
            client.DownloadFile(line.TaskUrl.Value, imagePath);
        }
        catch (WebException)
        {
            return new UnableToReachWebsiteWarning(line.TaskUrl.ValueIndex, line.LineNumber);
        }

        return null;
    }
}