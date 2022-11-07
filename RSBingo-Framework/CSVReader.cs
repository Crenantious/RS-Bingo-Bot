// <copyright file="CSVReader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using RSBingo_Framework.Exceptions;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using static RSBingo_Framework.DAL.DataFactory;
    using static RSBingo_Framework.Records.BingoTaskRecord;

    /// <summary>
    /// Reads the "Task restrictions.csv" and "Tasks.csv" files, parses them then saves the info to the database.<br/>
    /// </summary>
    public class CSVReader
    {
        private static readonly string[] ValidImageExtensions = new string[] { ".jpg", ".bmp", ".png" };
        private static readonly string TaskRestictionsFileName = "Task restrictions.csv";
        private static readonly string TasksFileName = "Tasks.csv";
        private static readonly IDataWorker DataWorker = CreateDataWorker();
        private static readonly Dictionary<string, int> RestrictionNameToId = new ();

        private static string currentFileName = string.Empty;
        private static int currentFileLine = 1;

        public static void Run()
        {
            Console.WriteLine($"Reading {TaskRestictionsFileName} and {TasksFileName} files...");
            CheckFilesExist();
            ClearDBEntries();
            Console.WriteLine($"Parsing {TaskRestictionsFileName} and {TasksFileName} files...");
            //ParseFile(TaskRestictionsFileName, 2, CreateTaskRestriction);
            //dataWorker.SaveChanges();
            ParseFile(TasksFileName, 2, CreateTask);
            DataWorker.SaveChanges();
        }

        private static void CheckFilesExist()
        {
            if (!File.Exists(GetFilePath(TasksFileName))) { FileNotFoundException(TasksFileName); }
            if (!File.Exists(GetFilePath(TaskRestictionsFileName))) { FileNotFoundException(TaskRestictionsFileName); }
        }

        private static void FileNotFoundException(string fileName) =>
            throw new FileNotFoundException($"Cannot find the {fileName} file. Make sure it exists in the project root folder.");

        private static void ClearDBEntries()
        {
            // TODO: delete all restrictions
            DataWorker.BingoTasks.DeleteAll();
            DataWorker.SaveChanges();
            DataWorker.Context.Database.ExecuteSqlRaw("ALTER TABLE task AUTO_INCREMENT = 1");
        }

        public static string GetFilePath(string fileName) =>
            Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, fileName);

        /// <summary>
        /// Parses the CSV file and calls <paramref name="action"/> on each line.
        /// </summary>
        /// <param name="fileName">Name of the file to parse.</param>
        /// <param name="valueAmount">Amount of values each line of the file should have.</param>
        /// <param name="action">Called on each line of the file with each value of the line an element in the array parameter (in order).</param>
        private static void ParseFile(string fileName, int valueAmount, Action<string[]> action)
        {
            currentFileName = fileName;
            string filePath = GetFilePath(fileName);

            using (StreamReader reader = new (filePath))
            {
                currentFileLine = 1;
                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine() !;
                    string[]? values = line.Split(',');

                    if (values.Length != valueAmount)
                    {
                        ThrowIncorrectNumberOfValuesException(valueAmount, values.Length);
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

        private static void ThrowIncorrectNumberOfValuesException(int expectedValueCount,
            int actualValueCount)
        {
            throw new FormatException(GetFileExceptionMessage($"Expected {expectedValueCount} values but found {actualValueCount}."));
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
                throw new DuplicateRestrictionNameException(GetFileExceptionMessage("Duplicate restriction name found"));
            }

            Restriction restriction = DataWorker.Restrictions.Create(values[1]);
            RestrictionNameToId.Add(values[0], restriction.RowId);
        }

        /// <summary>
        /// "Tasks.csv" must have the format: name, difficulty, restriction name.
        /// </summary>
        /// <param name="values">A line of values in the "Tasks.csv" file.</param>
        private static void CreateTask(string[] values)
        {
            //if (!RestrictionNameToId.ContainsKey(values[2]))
            //{
            //    throw new RestrictionNameNotFoundException(GetFileExceptionMessage("Restriction name was not found"));
            //}

            Difficulty difficulty = StringToDifficulty(values[1]);
            DataWorker.BingoTasks.Create(values[0], difficulty);
        }

        private static Difficulty StringToDifficulty(string name)
        {
            name = name.ToLower();

            foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
            {
                if (name == difficulty.ToString().ToLower())
                {
                    return difficulty;
                }
            }
            throw new InvalidDifficultyException(GetFileExceptionMessage("Invalid difficulty found"));
        }

        private static string GetFileExceptionMessage(string prefix) =>
            $"{prefix} in {currentFileName} on line {currentFileLine}.";
    }
}
