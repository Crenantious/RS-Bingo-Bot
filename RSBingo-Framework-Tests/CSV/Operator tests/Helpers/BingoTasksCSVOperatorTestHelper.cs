// <copyright file="BingoTasksCSVOperatorTestHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Models;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework_Tests.DTO;
using RSBingo_Framework_Tests.CSV.LocalServer;
using static RSBingo_Common.General;

public class BingoTasksCSVOperatorTestHelper
{
    public static ReaderResults<LineType> CreateAndParseTasksInCSVFile<LineType>(params TaskInfo[] tasks)
        where LineType : CSVLine =>
        CSVReaderTestHelper.CreateAndParseCSVFile<LineType>(tasks.Select(t =>
            $"{t.Name}, {t.Difficulty}, {t.Amount}" +
            (t.ImageURL is null ? "" : $", {t.ImageURL}"))
            .ToArray());

    public static void CreateTasksInDB(IDataWorker dataWorker, params TaskInfo[] tasks)
    {
        foreach (TaskInfo task in tasks)
        {
            dataWorker.BingoTasks.CreateMany(task.Name, task.Difficulty, task.Amount);
        }
        dataWorker.SaveChanges();
    }

    public static void AssertTasks(IDataWorker dataWorkerAfter, params TaskInfo[] expectedTasksInDB)
    {
        int expectedNumberOfTasks = 0;

        foreach (TaskInfo taskInfo in expectedTasksInDB)
        {
            IEnumerable<BingoTask> tasks = dataWorkerAfter.BingoTasks.GetByNameAndDifficulty(taskInfo.Name, taskInfo.Difficulty);
            expectedNumberOfTasks += taskInfo.Amount;
            Assert.AreEqual(taskInfo.Amount, tasks.Count());
        }

        Assert.AreEqual(expectedNumberOfTasks, dataWorkerAfter.BingoTasks.CountAll());
    }

    public static void TestCleanup(params TaskInfo[] tasks)
    {
        foreach (TaskInfo task in tasks)
        {
            if (task.ImageURL is not null)
            {
                try { File.Delete(GetTaskImagePath(task.Name)); }
                catch { }
            }
        }
    }
}