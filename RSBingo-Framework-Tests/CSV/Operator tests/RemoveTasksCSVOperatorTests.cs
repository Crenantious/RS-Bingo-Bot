// <copyright file="RemoveTasksCSVOperatorTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework_Tests.DTO;
using RSBingoBot.CSV;
using RSBingoBot.CSV.Lines;
using RSBingoBot.CSV.Operators.Warnings;
using static RSBingo_Framework.Records.BingoTaskRecord;
using static RSBingoBot.CSV.Lines.AddOrRemoveTasksCSVLine;

[TestClass]
public class RemoveTasksCSVOperatorTests : MockDBBaseTestClass
{
    private IDataWorker dataWorkerBefore = null!;
    private IDataWorker dataWorkerAfter = null!;
    private RemoveTasksCSVOperator csvOperator = null!;
    private CSVData<RemoveTasksCSVLine> parsedCSVData = null!;

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

    [TestMethod]
    public void AddATaskToDBAndFile_ParseAndOperate_RemovedFromDBCorrectlyWithNoWarnings()
    {
        TaskInfo taskInfo = new("Task 1", Difficulty.Easy, MinNumberOfTasks);
        CreateTasksInDB(taskInfo);
        CreateAndParseTasksInCSVFile(taskInfo);

        Operate();

        AssertOperatorWarnings();
        AssertTasks();
    }

    [TestMethod]
    public void AddATaskToDBAndOneToFileWithADifferentName_ParseAndOperate_NotRemovedFromDBWithAWarning()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks);
        TaskInfo taskInfo2 = new("Task 2", Difficulty.Easy, MinNumberOfTasks);
        CreateTasksInDB(taskInfo1);
        CreateAndParseTasksInCSVFile(taskInfo2);

        Operate();

        AssertOperatorWarnings(typeof(TaskDoesNotExistWarning));
        AssertTasks(taskInfo1);
    }

    [TestMethod]
    public void AddATaskToDBAndOneToFileWithADifferentDifficulty_ParseAndOperate_NotRemovedFromDBWithAWarning()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks);
        TaskInfo taskInfo2 = new("Task 1", Difficulty.Hard, MinNumberOfTasks);
        CreateTasksInDB(taskInfo1);
        CreateAndParseTasksInCSVFile(taskInfo2);

        Operate();

        AssertOperatorWarnings(typeof(TaskDoesNotExistWarning));
        AssertTasks(taskInfo1);
    }

    [TestMethod]
    public void AddATaskToDBAndOneToFileWithAGreaterAmountThanInDB_ParseAndOperate_RemovedFromDBCorrectlyWithNoWarnings()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks);
        TaskInfo taskInfo2 = new("Task 1", Difficulty.Easy, MaxNumberOfTasks);
        CreateTasksInDB(taskInfo1);
        CreateAndParseTasksInCSVFile(taskInfo2);

        Operate();

        AssertOperatorWarnings();
        AssertTasks();
    }

    [TestMethod]
    public void AddATaskToDBAndOneToFileWithASmallerAmountThanInDB_ParseAndOperate_RemovedFromDBCorrectlyWithNoWarnings()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MaxNumberOfTasks);
        TaskInfo taskInfo2 = new("Task 1", Difficulty.Easy, MaxNumberOfTasks * 2);
        TaskInfo taskInfo3 = new("Task 1", Difficulty.Easy, MaxNumberOfTasks * 3);
        CreateTasksInDB(taskInfo3);
        CreateAndParseTasksInCSVFile(taskInfo1);

        Operate();

        AssertOperatorWarnings();
        AssertTasks(taskInfo2);
    }

    #region Private

    private void CreateTasksInDB(params TaskInfo[] taskInfos) =>
        BingoTasksCSVOperatorTestHelper.CreateTasksInDB(dataWorkerBefore, taskInfos);

    private void CreateAndParseTasksInCSVFile(params TaskInfo[] tasks) =>
        parsedCSVData = CSVReaderTestHelper.CreateAndParseCSVFile<RemoveTasksCSVLine>(tasks.Select(t =>
            $"{t.Name}, {t.Difficulty}, {t.Amount}").ToArray());

    private void Operate() =>
        csvOperator.Operate(parsedCSVData);

    private void AssertOperatorWarnings(params Type[] warningTypes) =>
        CollectionAssert.AreEqual(warningTypes, csvOperator.GetRawWarnings().Select(w => w.GetType()).ToArray());

    private void AssertTasks(params TaskInfo[] tasks) =>
        BingoTasksCSVOperatorTestHelper.AssertTasks(dataWorkerAfter, tasks);

    #endregion
}