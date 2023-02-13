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

    protected override void OperateOnLine(AddTaskCSVLine line)
    {
        DownloadTaskImage(line);
        dataWorker.BingoTasks.CreateMany(line.TaskName,
            line.TaskDifficulty,
            line.AmountOfTasks);
    }

    protected override void OnPostOperating() =>
        dataWorker.SaveChanges();

    private void DownloadTaskImage(AddTaskCSVLine line)
    {
        try
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(line.TaskName, GetTaskImagePath(line.TaskName));
            }
        }
        catch (NotSupportedException e)
        {
            throw new CSVReaderException("Image format is unsupported");
        }
        catch (WebException e)
        {
            throw new CSVReaderException("Unable to download the image");
        }
        catch (UnknownImageFormatException e)
        {
            throw new CSVReaderException("Unknown image format");
        }
        catch (InvalidImageContentException e)
        {
            throw new CSVReaderException("Invalid image content");
        }
    }
}