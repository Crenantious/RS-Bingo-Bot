// <copyright file="AddTasksCSVOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using System.Net;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.CSV.Operators.Warnings;
using static RSBingo_Common.General;
using static RSBingo_Framework.DAL.DataFactory;
using System;
using System.Collections.Immutable;
using RSBingo_Framework.Exceptions;
using RSBingo_Common;

/// <inheritdoc/>
public class AddTasksCSVOperator : CSVOperator<AddTasksCSVLine>
{
    private const string UnpermittedURLExceptionMessage = "The given URL: {0} is not a part of a white listed domain.";
    private const string UnableToReachWebstieExceptionMessage = "Unable to reach the given website: {0}.";

    public AddTasksCSVOperator(IDataWorker dataWorker)
        : base(dataWorker)
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

    private Warning? DownloadTaskImage(AddTasksCSVLine line, string imagePath)
    {
        try
        {
            if (WhitelistChecker.IsUrlWhitelisted(line.TaskUrl.Value) is false)
            {
                throw new UnpermittedURLException(UnableToReachWebstieExceptionMessage.FormatConst(line.TaskUrl.Value));
            }

            // TODO: Make this a subroutine to attempt to download multiple times should it fail. Throw after a given number of failures.
            WebClient client = new();
            client.DownloadFile(line.TaskUrl.Value, imagePath);
        }
        catch (WebException)
        {
            throw new UnableToReachWebsiteException(UnableToReachWebstieExceptionMessage);
        }

        return null;
    }
}