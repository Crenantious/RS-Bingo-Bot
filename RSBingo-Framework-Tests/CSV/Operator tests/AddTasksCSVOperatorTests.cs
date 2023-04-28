// <copyright file="AddTasksCSVOperatorTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.CSV.Operators.Warnings;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework_Tests.DTO;
using RSBingo_Framework_Tests.CSV.LocalServer;
using static RSBingo_Framework.Records.BingoTaskRecord;
using static RSBingo_Framework.CSV.Lines.AddOrRemoveTasksCSVLine;

[TestClass]
public class AddTasksCSVOperatorTests : MockDBBaseTestClass
{
    private IDataWorker dataWorkerBefore = null!;
    private IDataWorker dataWorkerAfter = null!;
    private AddTasksCSVOperator csvOperator = null!;
    private OperatorResults operatorResults = null!;
    private ReaderResults<AddTasksCSVLine> readerResults = null!;

    private string ValidImageURL = LocalTestServer.GetUrl<ValidImagePage>();
    private string CorruptImageURL = LocalTestServer.GetUrl<CorruptImagePage>();
    private string InvalidImageFormatURL = LocalTestServer.GetUrl<InvalidImageFormatPage>();
    private string InvalidURL = LocalTestServer.InvalidURL;

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
        dataWorkerBefore = CreateDW();
        dataWorkerAfter = CreateDW();
        csvOperator = new(dataWorkerBefore);
    }

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

        AssertReader(null);
        AssertOperator(null);
        AssertTasks(taskInfo);
    }

    [TestMethod]
    public void AddMinOfATaskToFile_ParseAndOperate_AddedToDBCorrectlyWithNoExceptionsOrWarnings()
    {
        TaskInfo taskInfo = new("Task 1", Difficulty.Easy, MinNumberOfTasks, ValidImageURL);
        CreateAndParseTasksInCSVFile(taskInfo);

        Operate();

        AssertReader(null);
        AssertOperator(null);
        AssertTasks(taskInfo);
    }

    [TestMethod]
    public void AddTasksWithSameNameAndDifferentDifficultiesToFile_ParseAndOperate_AddedToDBCorrectlyWithNoExceptionsOrWarnings()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks, ValidImageURL);
        TaskInfo taskInfo2 = new("Task 1", Difficulty.Medium, MinNumberOfTasks, ValidImageURL);
        CreateAndParseTasksInCSVFile(taskInfo1, taskInfo2);

        Operate();

        AssertReader(null);
        AssertOperator(null);
        AssertTasks(taskInfo1, taskInfo2);
    }

    [TestMethod]
    public void AddTasksWithDifferentNamesAndSameDifficultyToFile_ParseAndOperate_AddedToDBCorrectlyWithNoExceptionsOrWarnings()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks, ValidImageURL);
        TaskInfo taskInfo2 = new("Task 2", Difficulty.Easy, MinNumberOfTasks, ValidImageURL);
        CreateAndParseTasksInCSVFile(taskInfo1, taskInfo2);

        Operate();

        AssertReader(null);
        AssertOperator(null);
        AssertTasks(taskInfo1, taskInfo2);
    }

    [TestMethod]
    public void AddTasksWithSameNameAndSameDifficultyToFile_ParseAndOperate_AddedToDBCorrectlyWithNoExceptionsOrWarnings()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks, ValidImageURL);
        TaskInfo taskInfo2 = new("Task 1", Difficulty.Easy, MinNumberOfTasks * 2, ValidImageURL);
        CreateAndParseTasksInCSVFile(taskInfo1, taskInfo1);

        Operate();

        AssertReader(null);
        AssertOperator(null);
        AssertTasks(taskInfo2);
    }

    [TestMethod]
    public void AddTasksToDBAndToFile_ParseAndOperate_AddedToDBCorrectlyWithNoExceptionsOrWarnings()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks, ValidImageURL);
        TaskInfo taskInfo2 = new("Task 1", Difficulty.Easy, MinNumberOfTasks * 2, ValidImageURL);
        CreateTasksInDB(taskInfo1);
        CreateAndParseTasksInCSVFile(taskInfo1);

        Operate();

        AssertReader(null);
        AssertOperator(null);
        AssertTasks(taskInfo2);
    }

    [TestMethod]
    public void AddTasksWithAnInvalidURLToFile_ParseAndOperate_NotAddedToDBWithAWarningAndNoExceptions()
    {
        TaskInfo taskInfo = new TaskInfo("Task 1", Difficulty.Easy, MinNumberOfTasks, InvalidURL);
        CreateAndParseTasksInCSVFile(taskInfo);

        Operate();

        AssertReader(null);
        AssertOperator(null, typeof(UnableToReachWebsiteWarning));
        AssertTasks();
    }

    [TestMethod]
    public void AddTasksWithACorruptImageURLToFile_ParseAndOperate_NotAddedToDBWithAWarningAndNoExceptions()
    {
        TaskInfo taskInfo = new TaskInfo("Task 1", Difficulty.Easy, MinNumberOfTasks, CorruptImageURL);
        CreateAndParseTasksInCSVFile(taskInfo);

        Operate();

        AssertReader(null);
        AssertOperator(null, typeof(InvalidImageWarning));
        AssertTasks();
    }

    [TestMethod]
    public void AddTasksWithAnInvalidImageFormatURLToFile_ParseAndOperate_NotAddedToDBWithAWarningAndNoExceptions()
    {
        TaskInfo taskInfo = new TaskInfo("Task 1", Difficulty.Easy, MinNumberOfTasks, InvalidImageFormatURL);
        CreateAndParseTasksInCSVFile(taskInfo);

        Operate();

        AssertReader(null);
        AssertOperator(null, typeof(InvalidImageWarning));
        AssertTasks();
    }

    #region Private

    private void CreateAndParseTasksInCSVFile(params TaskInfo[] info) =>
        readerResults = BingoTasksCSVOperatorTestHelper.CreateAndParseTasksInCSVFile<AddTasksCSVLine>(info);

    private void Operate() =>
        operatorResults = CSVOperatorTestHelper.Operate(csvOperator, readerResults.data);

    private void CreateTasksInDB(params TaskInfo[] tasks) =>
        BingoTasksCSVOperatorTestHelper.CreateTasksInDB(dataWorkerBefore, tasks);

    private void AssertReader(Type? exceptionType) =>
        Assert.AreEqual(exceptionType, readerResults.exceptionType);

    private void AssertOperator(Type? exceptionType, params Type[] warningTypes) =>
         CSVOperatorTestHelper.AssertOperator(new(exceptionType, warningTypes.ToList()), operatorResults);

    private void AssertTasks(params TaskInfo[] info) =>
        BingoTasksCSVOperatorTestHelper.AssertTasks(dataWorkerAfter, info);

    #endregion
}