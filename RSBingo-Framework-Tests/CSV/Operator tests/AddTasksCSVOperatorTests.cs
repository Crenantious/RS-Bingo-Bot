// <copyright file="AddTasksCSVOperatorTests.cs" company="PlaceholderCompany">
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
public class AddTasksCSVOperatorTests : BingoTasksCSVOperatorTestHelper<AddTasksCSVOperator, AddTasksCSVLine>
{
    /// <summary>
    /// The server is opened here instead of before each test because it's possible that
    /// the url won't be released when the server tries to register it.
    /// </summary>
    static AddTasksCSVOperatorTests() =>
        LocalTestServer.Open();

    [TestMethod]
    public void AddMaxOfATaskToFile_ParseAndOperate_AddedToDBCorrectlyWithNoExceptionsOrWarnings()
    {
        TaskInfo taskInfo = new("Task 1", Difficulty.Easy, MaxNumberOfTasks, ValidImageURL);
        CreateAndParseTasksInCSVFile(taskInfo);

        Operate();

        AssertReaderAndOperator(null, null);
        AssertTasks(taskInfo);
    }

    [TestMethod]
    public void AddMinOfATaskToFile_ParseAndOperate_AddedToDBCorrectlyWithNoExceptionsOrWarnings()
    {
        TaskInfo taskInfo = new("Task 1", Difficulty.Easy, MinNumberOfTasks, ValidImageURL);
        CreateAndParseTasksInCSVFile(taskInfo);

        Operate();

        AssertReaderAndOperator(null, null);
        AssertTasks(taskInfo);
    }

    [TestMethod]
    public void AddTasksWithSameNameAndDifferentDifficultiesToFile_ParseAndOperate_AddedToDBCorrectlyWithNoExceptionsOrWarnings()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks, ValidImageURL);
        TaskInfo taskInfo2 = new("Task 1", Difficulty.Medium, MinNumberOfTasks, ValidImageURL);
        CreateAndParseTasksInCSVFile(taskInfo1, taskInfo2);

        Operate();

        AssertReaderAndOperator(null, null);
        AssertTasks(taskInfo1, taskInfo2);
    }

    [TestMethod]
    public void AddTasksWithDifferentNamesAndSameDifficultyToFile_ParseAndOperate_AddedToDBCorrectlyWithNoExceptionsOrWarnings()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks, ValidImageURL);
        TaskInfo taskInfo2 = new("Task 2", Difficulty.Easy, MinNumberOfTasks, ValidImageURL);
        CreateAndParseTasksInCSVFile(taskInfo1, taskInfo2);

        Operate();

        AssertReaderAndOperator(null, null);
        AssertTasks(taskInfo1, taskInfo2);
    }

    [TestMethod]
    public void AddTasksWithSameNameAndSameDifficultyToFile_ParseAndOperate_AddedToDBCorrectlyWithNoExceptionsOrWarnings()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks, ValidImageURL);
        TaskInfo taskInfo2 = new("Task 1", Difficulty.Easy, MinNumberOfTasks * 2, ValidImageURL);
        CreateAndParseTasksInCSVFile(taskInfo1, taskInfo1);

        Operate();

        AssertReaderAndOperator(null, null);
        AssertTasks(taskInfo2);
    }

    [TestMethod]
    public void AddTasksToDbAndToFile_ParseAndOperate_AddedToDBCorrectlyWithNoExceptionsOrWarnings()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks, ValidImageURL);
        TaskInfo taskInfo2 = new("Task 1", Difficulty.Easy, MinNumberOfTasks * 2, ValidImageURL);
        DataWorkerBefore.BingoTasks.CreateMany(taskInfo1.Name, taskInfo1.Difficulty, taskInfo1.Amount);
        DataWorkerBefore.SaveChanges();
        CreateAndParseTasksInCSVFile(taskInfo1);

        Operate();

        AssertReaderAndOperator(null, null);
        AssertTasks(taskInfo2);
    }

    [TestMethod]
    public void AddTasksWithAnInvalidURLToFile_ParseAndOperate_NotAddedToDBAndGetAWarningAndNoExceptions()
    {
        TaskInfo taskInfo = new TaskInfo("Task 1", Difficulty.Easy, MinNumberOfTasks, InvalidURL);
        CreateAndParseTasksInCSVFile(taskInfo);

        Operate();

        AssertReaderAndOperator(null, null, typeof(UnableToReachWebsiteWarning));
        AssertTasks();
    }

    [TestMethod]
    public void AddTasksWithACorruptImageURLToFile_ParseAndOperate_NotAddedToDBAndGetAWarningAndNoExceptions()
    {
        TaskInfo taskInfo = new TaskInfo("Task 1", Difficulty.Easy, MinNumberOfTasks, CorruptImageURL);
        CreateAndParseTasksInCSVFile(taskInfo);

        Operate();

        AssertReaderAndOperator(null, null, typeof(InvalidImageWarning));
        AssertTasks();
    }

    [TestMethod]
    public void AddTasksWithAnInvalidImageFormatURLToFile_ParseAndOperate_NotAddedToDBAndGetAWarningAndNoExceptions()
    {
        TaskInfo taskInfo = new TaskInfo("Task 1", Difficulty.Easy, MinNumberOfTasks, InvalidImageFormatURL);
        CreateAndParseTasksInCSVFile(taskInfo);

        Operate();

        AssertReaderAndOperator(null, null, typeof(InvalidImageWarning));
        AssertTasks();
    }
}