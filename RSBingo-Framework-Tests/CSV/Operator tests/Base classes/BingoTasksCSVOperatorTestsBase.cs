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

[TestClass]
public class BingoTasksCSVOperatorTestsBase<CSVOperatorType, CSVLineType> : CSVOperatorTestsBase<CSVOperatorType, CSVLineType>
    where CSVOperatorType : CSVOperator<CSVLineType>, new()
    where CSVLineType : CSVLine
{
    private readonly List<string> filesToCleanup = new();

    protected static string ValidImageURL { get; } = LocalTestServer.GetUrl<ValidImagePage>();
    protected static string CorruptImageURL { get; } = LocalTestServer.GetUrl<CorruptImagePage>();
    protected static string InvalidImageFormatURL { get; } = LocalTestServer.GetUrl<InvalidImageFormatPage>();
    protected static string InvalidURL { get; } = LocalTestServer.InvalidURL;

    protected record TaskInfo(string Name, Difficulty Difficulty, int Amount, string? ImageURL = null);

    [TestCleanup]
    public override void TestCleanup()
    {
        foreach (string fileName in filesToCleanup)
        {
            try { File.Delete(fileName); }
            catch { }
        }
    }

    protected void AssertTasks(params TaskInfo[] expectedTasksInDB)
    {
        int expectedNumberOfTasks = 0;

        foreach (TaskInfo taskInfo in expectedTasksInDB)
        {
            IEnumerable<BingoTask> tasks = DataWorkerAfter.BingoTasks.GetByNameAndDifficulty(taskInfo.Name, taskInfo.Difficulty);
            expectedNumberOfTasks += taskInfo.Amount;
            Assert.AreEqual(taskInfo.Amount, tasks.Count());

            foreach (BingoTask task in tasks)
            {
                if (taskInfo.ImageURL is not null) { filesToCleanup.Add(GetTaskImagePath(taskInfo.Name)); }
            }
        }

        Assert.AreEqual(expectedNumberOfTasks, DataWorkerAfter.BingoTasks.CountAll());
    }

    protected void CreateAndParseTasksInCSVFile(params TaskInfo[] tasks) =>
        CreateAndParseCSVFile(tasks.Select(t => 
            $"{t.Name}, {t.Difficulty}, {t.Amount}" + 
            (t.ImageURL is null ? "" : $", {t.ImageURL}"))
            .ToArray());
}