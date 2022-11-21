﻿// <copyright file="CSVReader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.EntityFrameworkCore;
    using RSBingo_Common;
    using RSBingo_Framework.Exceptions;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Processing;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Formats;
    using static RSBingo_Framework.DAL.DataFactory;
    using static RSBingo_Framework.Records.BingoTaskRecord;
    using static RSBingo_Common.General;

    /// <summary>
    /// Reads and parses a csv file, then updates the database accordingly.<br/>
    /// </summary>
    public abstract class CSVReader
    {
        private const string taskImageExtension = ".png";

        private static readonly IDataWorker DataWorker = CreateDataWorker();
        private static readonly Dictionary<string, int> RestrictionNameToId = new ();
        private static readonly HashSet<int> TaskIds = new ();

        private static int currentFileLine = 1;

        static CSVReader()
        {
            // Reset auto increment just in case it overflows
            DataWorker.Context.Database.ExecuteSqlRaw("ALTER TABLE task AUTO_INCREMENT = 1");
        }

        /// <summary>
        /// Attempts to add tasks to the database based on a csv file.
        /// </summary>
        /// <param name="csvUrl">The URL of the csv file to parse.</param>
        /// <returns>An error message to display to the user attempting to add tasks,
        /// or <see cref="string.Empty"/> if no errors occurred.</returns>
        public static string CreateTasks(string csvUrl) =>
            AddDeleteTasks(csvUrl, true);

        /// <summary>
        /// Attempts to delete tasks to the database based on a csv file.
        /// </summary>
        /// <param name="csvUrl">The URL of the csv file to parse.</param>
        /// <returns>An error message to display to the user attempting to delete tasks,
        /// or <see cref="string.Empty"/> if no errors occurred.</returns>
        public static string DeleteTasks(string csvUrl) =>
            AddDeleteTasks(csvUrl, false);

        private static string AddDeleteTasks(string csvUrl, bool addTasks)
        {
            using (var client = new WebClient())
            {
                string TasksFileName = "Tasks.csv";

                try
                {
                    client.DownloadFile(csvUrl, TasksFileName);

                    ParseFile(TasksFileName,
                        addTasks ? 4 : 3,
                        addTasks ? CreateTask : DeleteTask);
                    DataWorker.SaveChanges();
                }
                catch (Exception e)
                {
                    if (e.GetType() == typeof(CSVReaderException))
                    {
                        return e.Message;
                    }
                    else
                    {
                        General.LoggingLog(e, e.Message);
                        return "Internal error.";
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Parses the CSV file and calls <paramref name="action"/> on each line.
        /// </summary>
        /// <param name="fileName">Name of the file to parse.</param>
        /// <param name="valueAmount">Amount of values each line of the file should have.</param>
        /// <param name="action">Called on each line of the file with each value of the line an element in the array parameter (in order).</param>
        private static void ParseFile(string fileName, int valueAmount, Action<string[]> action)
        {
            // TODO: JR - add ability for unlimited parameters, or an array of options for a parameter
            // (task restrictions in this case).

            using (StreamReader reader = new (fileName))
            {
                currentFileLine = 1;
                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine() !;
                    string[]? values = line.Split(',');

                    if (values.Length < valueAmount)
                    {
                        throw new CSVReaderException(GetFileExceptionMessage($"Expected at least {valueAmount} values but found {values.Length}"));
                    }

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (values[i][0] is ' ') { values[i] = values[i][1..]; }
                    }

                    action(values);
                    currentFileLine++;
                }
            }
        }

        /// <summary>
        /// "Task restrictions.csv" must have the format: name, description.
        /// </summary>
        /// <param name="values">A line of values in the "Tasks.csv" file.</param>
        /// <exception cref="DuplicateRestrictionNameException">
        /// Thrown if there are duplicate restriction names in <paramref name="values"/>.
        /// </exception>
        private static void CreateTaskRestriction(string[] values)
        {
            if (RestrictionNameToId.ContainsKey(values[0]))
            {
                throw new CSVReaderException(GetFileExceptionMessage("Duplicate restriction name found"));
            }

            Restriction restriction = DataWorker.Restrictions.Create(values[1]);
            RestrictionNameToId.Add(values[0], restriction.RowId);
        }

        private static void CreateTask(string[] values)
        {
            string url = GetTaskImageUrl(values);
            if (url is "")
            {
                throw new CSVReaderException(GetFileExceptionMessage("Image url cannot be empty"));
            }

            string taskName = GetTaskName(values);
            string imagePath = GetTaskImagePath(taskName);

            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(url, imagePath);
                }
            }
            catch (NotSupportedException e)
            {
                throw new CSVReaderException(GetFileExceptionMessage("Image format is unsupported"));
            }
            catch (WebException e)
            {
                throw new CSVReaderException(GetFileExceptionMessage("Unable to download the image"));
            }
            catch (UnknownImageFormatException e)
            {
                throw new CSVReaderException(GetFileExceptionMessage("Unknown image format"));
            }
            catch (InvalidImageContentException e)
            {
                throw new CSVReaderException(GetFileExceptionMessage("Invalid image content"));
            }

            DataWorker.BingoTasks.CreateMany(taskName,
                GetTaskDifficulty(values),
                GetTaskAmount(values));
        }

        private static void DeleteTask(string[] values)
        {
            DataWorker.BingoTasks.DeleteMany(
                DataWorker.BingoTasks.GetByNameAndDifficulty(GetTaskName(values), GetTaskDifficulty(values))
                .Take(GetTaskAmount(values)));
        }

        private static string GetTaskName(string[] values) =>
            values[0];

        private static Difficulty GetTaskDifficulty(string[] values)
        {
            string name = values[1].ToLower();

            foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
            {
                if (name == difficulty.ToString().ToLower())
                {
                    return difficulty;
                }
            }

            throw new CSVReaderException(GetFileExceptionMessage("Invalid difficulty found"));
        }

        private static int GetTaskAmount(string[] values)
        {
            int numberOfTiles;

            try
            {
                numberOfTiles = int.Parse(values[2]);
            }
            catch
            {
                throw new CSVReaderException(GetFileExceptionMessage("The 'number of tiles' argument must be an integer"));
            }

            if (numberOfTiles <= 0)
            {
                throw new CSVReaderException(GetFileExceptionMessage("The 'number of tiles' argument must be greater than 0"));
            }

            return numberOfTiles;
        }

        private static string GetTaskImageUrl(string[] values)
        {
            //if (!RestrictionNameToId.ContainsKey(values[2]))
            //{
            //    throw new RestrictionNameNotFoundException(GetFileExceptionMessage("Restriction name was not found"));
            //}

            return values[3];
        }

        private static string GetFileExceptionMessage(string prefix) =>
            $"{prefix} on line {currentFileLine}.";
    }
}
