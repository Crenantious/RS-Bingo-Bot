// <copyright file="RemoveTasksCSVOperatorTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.CSV.Operators.Warnings;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework_Tests.DTO;
using static RSBingo_Framework.Records.BingoTaskRecord;
using static RSBingo_Framework.CSV.Lines.AddOrRemoveTasksCSVLine;

[TestClass]
public class RemoveTasksCSVOperatorTests : MockDBBaseTestClass
{
    private IDataWorker dataWorkerBefore = null!;
    private IDataWorker dataWorkerAfter = null!;
    private RemoveTasksCSVOperator csvOperator = null!;
    private OperatorResults operatorResults = null!;
    private ReaderResults<RemoveTasksCSVLine> readerResults = null!;

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
        dataWorkerBefore = CreateDW();
        dataWorkerAfter = CreateDW();
        csvOperator = new(dataWorkerBefore);
    }

    [TestMethod]
    public void AddATaskToDBAndFile_ParseAndOperate_RemovedFromDBCorrectlyWithNoWarningsOrExceptions()
    {
        TaskInfo taskInfo = new("Task 1", Difficulty.Easy, MinNumberOfTasks);
        CreateTasksInDB(taskInfo);
        CreateAndParseTasksInCSVFile(taskInfo);

        Operate();

        AssertReader(null);
        AssertOperator(null);
        AssertTasks();
    }

    [TestMethod]
    public void AddATaskToDBAndOneToFileWithADifferentName_ParseAndOperate_NotRemovedFromDBWithAWarningAndNoExceptions()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks);
        TaskInfo taskInfo2 = new("Task 2", Difficulty.Easy, MinNumberOfTasks);
        CreateTasksInDB(taskInfo1);
        CreateAndParseTasksInCSVFile(taskInfo2);

        Operate();

        AssertReader(null);
        AssertOperator(null, typeof(TaskDoesNotExistWarning));
        AssertTasks(taskInfo1);
    }

    [TestMethod]
    public void AddATaskToDBAndOneToFileWithADifferentDifficulty_ParseAndOperate_NotRemovedFromDBWithAWarningAndNoExceptions()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks);
        TaskInfo taskInfo2 = new("Task 1", Difficulty.Hard, MinNumberOfTasks);
        CreateTasksInDB(taskInfo1);
        CreateAndParseTasksInCSVFile(taskInfo2);

        Operate();

        AssertReader(null);
        AssertOperator(null, typeof(TaskDoesNotExistWarning));
        AssertTasks(taskInfo1);
    }

    // TOODO: FIX ME!

    [TestMethod]
    public void AddATaskToDBAndOneToFileWithAGreaterAmountThanInDB_ParseAndOperate_RemovedFromDBCorrectlyWithAWarningAndNoExceptions()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks);
        TaskInfo taskInfo2 = new("Task 1", Difficulty.Easy, MaxNumberOfTasks);
        CreateTasksInDB(taskInfo1);
        CreateAndParseTasksInCSVFile(taskInfo2);

        Operate();

        AssertReader(null);

        // AssertOperator(null, typeof(NotEnoughTasksToDeleteWarning));

        AssertTasks();
    }

    [TestMethod]
    public void AddATaskToDBAndOneToFileWithASmallerAmountThanInDB_ParseAndOperate_RemovedFromDBCorrectlyWithNoWarningsOrExceptions()
    {
        TaskInfo taskInfo1 = new("Task 1", Difficulty.Easy, MaxNumberOfTasks);
        TaskInfo taskInfo2 = new("Task 1", Difficulty.Easy, MaxNumberOfTasks * 2);
        TaskInfo taskInfo3 = new("Task 1", Difficulty.Easy, MaxNumberOfTasks * 3);
        CreateTasksInDB(taskInfo3);
        CreateAndParseTasksInCSVFile(taskInfo1);

        Operate();

        AssertReader(null);
        AssertOperator(null);
        AssertTasks(taskInfo2);
    }

    #region Private

    private void CreateAndParseTasksInCSVFile(params TaskInfo[] info) =>
        readerResults = BingoTasksCSVOperatorTestHelper.CreateAndParseTasksInCSVFile<RemoveTasksCSVLine>(info);

    private void Operate() =>
        operatorResults = CSVOperatorTestHelper.Operate(csvOperator, readerResults.data);

    private void CreateTasksInDB(params TaskInfo[] taskInfos) =>
        BingoTasksCSVOperatorTestHelper.CreateTasksInDB(dataWorkerBefore, taskInfos);

    private void AssertReader(Type? exceptionType) =>
    Assert.AreEqual(readerResults.exceptionType, exceptionType);

    private void AssertOperator(Type? exceptionType, params Type[] warningTypes) =>
         CSVOperatorTestHelper.AssertOperator(new(exceptionType, warningTypes.ToList()), operatorResults);

    private void AssertTasks(params TaskInfo[] info) =>
        BingoTasksCSVOperatorTestHelper.AssertTasks(dataWorkerAfter, info);

    #endregion
}