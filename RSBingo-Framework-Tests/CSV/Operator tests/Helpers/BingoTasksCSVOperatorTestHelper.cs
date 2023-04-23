// <copyright file="BingoTasksCSVOperatorTestsBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Models;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework_Tests.CSV.LocalServer;
using RSBingo_Framework.CSV.Operators.Warnings;
using static RSBingo_Common.General;
using static RSBingo_Framework.Records.BingoTaskRecord;
using static RSBingo_Framework.CSV.Lines.AddOrRemoveTasksCSVLine;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework_Tests.DTO;

public class BingoTasksCSVOperatorTestHelper
{
    private readonly List<string> filesToCleanup = new();

    public static string ValidImageURL { get; } = LocalTestServer.GetUrl<ValidImagePage>();
    public static string CorruptImageURL { get; } = LocalTestServer.GetUrl<CorruptImagePage>();
    public static string InvalidImageFormatURL { get; } = LocalTestServer.GetUrl<InvalidImageFormatPage>();
    public static string InvalidURL { get; } = LocalTestServer.InvalidURL;
    public void TestCleanup()
    {
        foreach (string fileName in filesToCleanup)
        {
            try { File.Delete(fileName); }
            catch { }
        }
    }

    public void AssertTasks(IDataWorker dataWorkerAfter, params TaskInfo[] expectedTasksInDB)
    {
        int expectedNumberOfTasks = 0;

        foreach (TaskInfo taskInfo in expectedTasksInDB)
        {
            IEnumerable<BingoTask> tasks = dataWorkerAfter.BingoTasks.GetByNameAndDifficulty(taskInfo.Name, taskInfo.Difficulty);
            expectedNumberOfTasks += taskInfo.Amount;
            Assert.AreEqual(taskInfo.Amount, tasks.Count());

            foreach (BingoTask task in tasks)
            {
                if (taskInfo.ImageURL is not null) { filesToCleanup.Add(GetTaskImagePath(taskInfo.Name)); }
            }
        }

        Assert.AreEqual(expectedNumberOfTasks, dataWorkerAfter.BingoTasks.CountAll());
    }

    public void CreateAndParseTasksInCSVFile(params TaskInfo[] tasks) =>
        CreateAndParseCSVFile(tasks.Select(t => 
            $"{t.Name}, {t.Difficulty}, {t.Amount}" + 
            (t.ImageURL is null ? "" : $", {t.ImageURL}"))
            .ToArray());
}