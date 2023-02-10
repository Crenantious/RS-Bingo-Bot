// <copyright file="AddTaskCSVReader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV_reader;

using System.Net;
using Microsoft.EntityFrameworkCore;
using RSBingo_Framework.Exceptions;
using SixLabors.ImageSharp;
using static RSBingo_Common.General;
using RSBingo_Framework.CSV_reader.CSV_lines;

/// <inheritdoc/>
public class AddTasksCSVReader : CSVReader<AddTaskCSVLine>
{
    public AddTasksCSVReader()
    {
        // Reset auto increment just in case it overflows
        // TODO: this should not be done here; move it to somewhere appropriate.
        DataWorker.Context.Database.ExecuteSqlRaw("ALTER TABLE task AUTO_INCREMENT = 1");
    }

    protected override void LineSuccessfullyParsed()
    {
        DownloadTaskImage();
        DataWorker.BingoTasks.CreateMany(Line.TaskName,
            Line.TaskDifficulty,
            Line.AmountOfTasks);
    }

    private void DownloadTaskImage()
    {
        try
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(Line.TaskName, GetTaskImagePath(Line.TaskName));
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

    protected override void PostParsing() =>
        DataWorker.SaveChanges();
}