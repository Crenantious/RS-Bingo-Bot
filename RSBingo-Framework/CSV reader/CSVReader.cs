// <copyright file="CSVReader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV_reader;

using System.Net;
using Microsoft.EntityFrameworkCore;
using RSBingo_Common;
using RSBingo_Framework.Exceptions;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using SixLabors.ImageSharp;
using static RSBingo_Framework.DAL.DataFactory;
using static RSBingo_Common.General;
using RSBingo_Framework.CSV_reader.CSV_lines;

/// <summary>
/// Reads and parses a CSV file, then updates the database accordingly.
/// </summary>
public abstract class CSVReader
{
    private const string taskImageExtension = ".png";

    private static readonly IDataWorker DataWorker = CreateDataWorker();
    private static readonly Dictionary<string, int> RestrictionNameToId = new();
    private static readonly HashSet<int> TaskIds = new();

    private static int currentFileLine = 1;

    static CSVReader()
    {
        // Reset auto increment just in case it overflows
        // TODO: this should not be done here; move it to somewhere appropriate.
        DataWorker.Context.Database.ExecuteSqlRaw("ALTER TABLE task AUTO_INCREMENT = 1");
    }

    /// <summary>
    /// Attempts to add tasks to the database.
    /// </summary>
    /// <param name="filePath">The path of the CSV file to parse.</param>
    /// <returns>An error message to display to the user, or <see langword="null"/> if no errors occurred.</returns>
    public static string? AddTasks(string filePath) =>
        ParseAndGetErrorMessage<AddOrRemoveTaskCSVLine>(filePath, new AddOrRemoveTaskCSVLine(), CreateTask);

    /// <summary>
    /// Attempts to remove tasks from the database.
    /// </summary>
    /// <param name="filePath">The path of the CSV file to parse.</param>
    /// <returns>An error message to display to the user, or <see langword="null"/> if no errors occurred.</returns>
    public static string RemoveTasks(string filePath) =>
        ParseAndGetErrorMessage<RemoveTaskCSVLine>(filePath, new RemoveTaskCSVLine(), DeleteTask);

    /// <summary>
    /// Attempts to add task restrictions to the database.
    /// </summary>
    /// <param name="filePath">The path of the CSV file to parse.</param>
    /// <returns>An error message to display to the user, or <see langword="null"/> if no errors occurred.</returns>
    public static string AddTaskRestrictions(string filePath) =>
        ParseAndGetErrorMessage<AddTaskRestrictionCSVLine>(filePath, new AddTaskRestrictionCSVLine(), AddTaskRestriction);

    private static string ParseAndGetErrorMessage<T>(string filePath, T CSVLineInstance, Action<T> action) where T : CSVLine
    {
        try
        {
            ParseFile(filePath, CSVLineInstance, action);
            DataWorker.SaveChanges();
        }
        catch (Exception e)
        {
            if (e.GetType() == typeof(CSVReaderException))
            {
                return GetFileExceptionMessage(e.Message);
            }
            else
            {
                General.LoggingLog(e, e.Message);
                return "Internal error.";
            }
        }
        return string.Empty;
    }

    /// <param name="CSVLineInstance">Required to ensure CSVLine is not passed as a type since it is abstract</param>
    /// <param name="action">Called on each line of the file with the CSVLineInstance updated accordingly.</param>
    private static void ParseFile<T>(string fileName, T CSVLineInstance, Action<T> action) where T : CSVLine
    {
        using (StreamReader reader = new(fileName))
        {
            currentFileLine = 1;

            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine()!;
                if (line == null) { throw new CSVReaderException("Unable to read"); }

                string[] values = line.Split(',');

                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i][0] is ' ') { values[i] = values[i][1..]; }
                }

                CSVLineInstance.Parse(values);
                action(CSVLineInstance);
                currentFileLine++;
            }
        }
    }

    private static void AddTaskRestriction(AddTaskRestrictionCSVLine line)
    {
        if (RestrictionNameToId.ContainsKey(line.RestrictionName))
        {
            throw new CSVReaderException("Duplicate restriction name found");
        }

        Restriction restriction = DataWorker.Restrictions.Create(line.RestrictionDescription);
        RestrictionNameToId.Add(line.RestrictionName, restriction.RowId);
    }

    private static void CreateTask(AddOrRemoveTaskCSVLine line)
    {
        // Download task image.
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

        DataWorker.BingoTasks.CreateMany(line.TaskName,
            line.TaskDifficulty,
            line.AmountOfTasks);
    }

    private static void DeleteTask(RemoveTaskCSVLine line)
    {
        DataWorker.BingoTasks.DeleteMany(
            DataWorker.BingoTasks.GetByNameAndDifficulty(line.TaskName, line.TaskDifficulty)
            .Take(line.AmountOfTasks));
    }

    private static string GetFileExceptionMessage(string prefix) =>
        $"{prefix} on line {currentFileLine}.";
}