// <copyright file="RemoveTasksCSVOperatorTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.Models;
using RSBingo_Framework.CSV.Operators.Warnings;
using static RSBingo_Common.General;
using static RSBingo_Framework.Records.BingoTaskRecord;
using static RSBingo_Framework.CSV.Lines.AddOrRemoveTasksCSVLine;
using static RSBingo_Framework_Tests.CSV.CSVOperatorTestHelper;
using RSBingo_Framework_Tests.DTO;
using RSBingo_Framework.Interfaces;

[TestClass]
public class RemoveTasksCSVOperatorTests : MockDBBaseTestClass
{
    private BingoTasksCSVOperatorTestHelper bingoTasksCSVOperatorTestHelper = null!;
    private IDataWorker dataWorkerBefore = null!;
    private IDataWorker dataWorkerAfter = null!;

    public override void TestInitialize()
    {
        dataWorkerBefore = CreateDW();
        dataWorkerAfter = CreateDW();
        bingoTasksCSVOperatorTestHelper = new();
        RemoveTaskRestrictionsCSVOperator removeTaskRestrictionsCSVOperator = new(dataWorkerBefore);
        base.TestInitialize();
    }

    [TestMethod]
    public void AddATaskToDBAndFile_ParseAndOperate_RemovedFromDBCorrectlyWithNoExceptionsOrWarnings()
    {
        TaskInfo task = new("Task 1", Difficulty.Easy, MinNumberOfTasks);
        CreateTasksInDB(task);
        CreateAndParseTasksInCSVFile(task);

        OperatorResults result = Operate();

        AssertOperator(null);
        CSVOperatorTestHelper.AssertTasks(dataWorkerAfter);
    }

    [TestMethod]
    public void AddATaskToDBAndOneToFileWithADifferentName_ParseAndOperate_NotRemovedFromDBAndGetAWarningAndNoExceptions()
    {
        TaskInfo task1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks);
        TaskInfo task2 = new("Task 2", Difficulty.Easy, MinNumberOfTasks);
        CreateTasksInDB(task1);
        CreateAndParseTasksInCSVFile(task2);

        Operate();

        AssertOperator(null, null, typeof(TaskDoesNotExistWarning));
        AssertTasks(task1);
    }

    [TestMethod]
    public void AddATaskToDBAndOneToFileWithADifferentDifficulty_ParseAndOperate_NotRemovedFromDBAndGetAWarningAndNoExceptions()
    {
        TaskInfo task1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks);
        TaskInfo task2 = new("Task 1", Difficulty.Hard, MinNumberOfTasks);
        CreateTasksInDB(task1);
        CreateAndParseTasksInCSVFile(task2);

        Operate();

        AssertOperator(null, null, typeof(TaskDoesNotExistWarning));
        AssertTasks(task1);
    }

    [TestMethod]
    public void AddATaskToDBAndOneToFileWithAGreaterAmountThanInDB_ParseAndOperate_RemovedFromDBCorrectlyAndGetAWarningAndNoExceptions()
    {
        TaskInfo task1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks);
        TaskInfo task2 = new("Task 1", Difficulty.Easy, MaxNumberOfTasks);
        CreateTasksInDB(task1);
        CreateAndParseTasksInCSVFile(task2);

        Operate();

        AssertOperator(null, null, typeof(NotEnoughTasksToDeleteWarning));
        AssertTasks();
    }

    [TestMethod]
    public void AddATaskToDBAndOneToFileWithASmallerAmountThanInDB_ParseAndOperate_RemovedFromDBCorrectlyAndGetAWarningAndNoExceptions()
    {
        TaskInfo task1 = new("Task 1", Difficulty.Easy, MaxNumberOfTasks);
        TaskInfo task2 = new("Task 1", Difficulty.Easy, MaxNumberOfTasks * 2);
        TaskInfo task3 = new("Task 1", Difficulty.Easy, MaxNumberOfTasks * 3);
        CreateTasksInDB(task3);
        CreateAndParseTasksInCSVFile(task1);

        Operate();

        AssertOperator();
        AssertTasks(task2);
    }

    private void CreateTasksInDB(params TaskInfo[] tasks)
    {
        foreach (TaskInfo task in tasks)
        {
            dataWorkerBefore.BingoTasks.CreateMany(task.Name, task.Difficulty, task.Amount);
        }

        dataWorkerBefore.SaveChanges();
    }

    #region Private

    private void CreateAndParseTasksInCSVFile(params TaskInfo[] info)
    {
        bingoTasksCSVOperatorTestHelper.CreateAndParseTasksInCSVFile(info);
    }

    private void AssertTasks(params TaskInfo[] info)
    {
        bingoTasksCSVOperatorTestHelper.AssertTasks(dataWorkerAfter, info);
    }

    #endregion
}