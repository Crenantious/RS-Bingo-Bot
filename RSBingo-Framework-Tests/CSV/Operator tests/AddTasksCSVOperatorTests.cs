// <copyright file="AddTasksCSVOperatorTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.CSV.Operators.Warnings;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Exceptions;
using RSBingo_Framework_Tests.DTO;
using RSBingo_Framework_Tests.CSV.LocalServer;
using static RSBingo_Framework.Records.BingoTaskRecord;
using static RSBingo_Framework.CSV.Lines.AddOrRemoveTasksCSVLine;

[TestClass]
public class AddTasksCSVOperatorTests : MockDBBaseTestClass
{
    private const string InvalidURL = LocalTestServer.UriPrefix + "This is an invalid url/";
    private const string UnpermittedURL = "http://google.com/";

    private IDataWorker dataWorkerBefore = null!;
    private IDataWorker dataWorkerAfter = null!;
    private AddTasksCSVOperator csvOperator = null!;
    private CSVData<AddTasksCSVLine> parsedCSVData = null!;

    private string ValidImageURL = LocalTestServer.GetUrl<ValidImagePage>();
    private string CorruptImageURL = LocalTestServer.GetUrl<CorruptImagePage>();
    private string InvalidImageFormatURL = LocalTestServer.GetUrl<InvalidImageFormatPage>();

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
        dataWorkerBefore = CreateDW();
        dataWorkerAfter = CreateDW();
        csvOperator = new(dataWorkerBefore);
    }

    [TestCleanup]
    public override void TestCleanup()
    {
        CSVReaderTestHelper.TestCleanup();
        BingoTasksCSVOperatorTestHelper.TestCleanup();
    }

    /// <summary>
    /// The server is opened here instead of before each test because it's possible that
    /// the url won't be released when the server tries to register it.
    /// </summary>
    static AddTasksCSVOperatorTests() =>
        LocalTestServer.Open();

    [TestMethod]
    public void AddMaxOfATaskToFile_ParseAndOperate_AddedToDBCorrectlyWithNoWarnings()
    {
        TaskInfo taskInfo = new("Task 1", Difficulty.Easy, MaxNumberOfTasks, ValidImageURL);
        CreateAndParseTasksInCSVFile(taskInfo);

        Operate();

        AssertOperatorWarnings();
        AssertTasks(taskInfo);
    }

    [TestMethod]
    public void AddMinOfATaskToFile_ParseAndOperate_AddedToDBCorrectlyWithNoWarnings()
    {
        TaskInfo taskInfo = new("Task 1", Difficulty.Easy, MinNumberOfTasks, ValidImageURL);
        CreateAndParseTasksInCSVFile(taskInfo);

        Operate();

        AssertOperatorWarnings();
        AssertTasks(taskInfo);
    }

    [TestMethod]
    public void AddTasksWithSameNameAndDifferentDifficultiesToFile_ParseAndOperate_AddedToDBCorrectlyWithNoWarnings()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks, ValidImageURL);
        TaskInfo taskInfo2 = new("Task 1", Difficulty.Medium, MinNumberOfTasks, ValidImageURL);
        CreateAndParseTasksInCSVFile(taskInfo1, taskInfo2);

        Operate();

        AssertOperatorWarnings();
        AssertTasks(taskInfo1, taskInfo2);
    }

    [TestMethod]
    public void AddTasksWithDifferentNamesAndSameDifficultyToFile_ParseAndOperate_AddedToDBCorrectlyWithNoWarnings()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks, ValidImageURL);
        TaskInfo taskInfo2 = new("Task 2", Difficulty.Easy, MinNumberOfTasks, ValidImageURL);
        CreateAndParseTasksInCSVFile(taskInfo1, taskInfo2);

        Operate();

        AssertOperatorWarnings();
        AssertTasks(taskInfo1, taskInfo2);
    }

    [TestMethod]
    public void AddTasksWithSameNameAndSameDifficultyToFile_ParseAndOperate_AddedToDBCorrectlyWithNoWarnings()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks, ValidImageURL);
        TaskInfo taskInfo2 = new("Task 1", Difficulty.Easy, MinNumberOfTasks * 2, ValidImageURL);
        CreateAndParseTasksInCSVFile(taskInfo1, taskInfo1);

        Operate();

        AssertOperatorWarnings();
        AssertTasks(taskInfo2);
    }

    [TestMethod]
    public void AddTasksToDBAndToFile_ParseAndOperate_AddedToDBCorrectlyWithNoWarnings()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks, ValidImageURL);
        TaskInfo taskInfo2 = new("Task 1", Difficulty.Easy, MinNumberOfTasks * 2, ValidImageURL);
        BingoTasksCSVOperatorTestHelper.CreateTasksInDB(dataWorkerBefore, taskInfo1);
        CreateAndParseTasksInCSVFile(taskInfo1);

        Operate();

        AssertOperatorWarnings();
        AssertTasks(taskInfo2);
    }

    [TestMethod]
    public void AddTasksWithAnInvalidURLToFile_ParseAndOperate_NotAddedToDBWithAWarning()
    {
        TaskInfo taskInfo = new TaskInfo("Task 1", Difficulty.Easy, MinNumberOfTasks, InvalidURL);
        CreateAndParseTasksInCSVFile(taskInfo);

        Assert.ThrowsException<UnableToReachWebsiteException>(Operate);

        AssertTasks();
    }

    [TestMethod]
    public void AddTasksWithAUnpermittedURLToFile_ParseAndOperate_NotAddedToDBWithAWarning()
    {
        TaskInfo taskInfo = new TaskInfo("Task 1", Difficulty.Easy, MinNumberOfTasks, UnpermittedURL);
        CreateAndParseTasksInCSVFile(taskInfo);

        Assert.ThrowsException<UnpermittedURLException>(Operate);

        AssertTasks();
    }

    [TestMethod]
    public void AddTasksWithACorruptImageURLToFile_ParseAndOperate_NotAddedToDBWithAWarning()
    {
        TaskInfo taskInfo = new TaskInfo("Task 1", Difficulty.Easy, MinNumberOfTasks, CorruptImageURL);
        CreateAndParseTasksInCSVFile(taskInfo);

        Operate();

        AssertOperatorWarnings(typeof(InvalidImageWarning));
        AssertTasks();
    }

    [TestMethod]
    public void AddTasksWithAnInvalidImageFormatURLToFile_ParseAndOperate_NotAddedToDBWithAWarning()
    {
        TaskInfo taskInfo = new TaskInfo("Task 1", Difficulty.Easy, MinNumberOfTasks, InvalidImageFormatURL);
        CreateAndParseTasksInCSVFile(taskInfo);

        Operate();

        AssertOperatorWarnings(typeof(InvalidImageWarning));
        AssertTasks();
    }

    #region Private

    private void CreateAndParseTasksInCSVFile(params TaskInfo[] tasks)
    {

        parsedCSVData = CSVReaderTestHelper.CreateAndParseCSVFile<AddTasksCSVLine>(tasks.Select(t =>
            $"{t.Name}, {t.Difficulty}, {t.Amount}, {t.ImageURL}").ToArray());
        BingoTasksCSVOperatorTestHelper.TasksToCleanUp = tasks.Select(t => t.Name);
    }

    private void Operate() =>
        csvOperator.Operate(parsedCSVData);

    private void AssertOperatorWarnings(params Type[] warningTypes) =>
        CollectionAssert.AreEqual(warningTypes, csvOperator.GetRawWarnings().Select(w => w.GetType()).ToArray());

    private void AssertTasks(params TaskInfo[] tasks) =>
        BingoTasksCSVOperatorTestHelper.AssertTasks(dataWorkerAfter, tasks);

    #endregion
}