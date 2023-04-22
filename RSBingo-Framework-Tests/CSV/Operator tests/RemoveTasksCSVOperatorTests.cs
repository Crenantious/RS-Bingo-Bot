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

[TestClass]
public class RemoveTasksCSVOperatorTests : BingoTasksCSVOperatorTestsBase<RemoveTasksCSVOperator, RemoveTasksCSVLine>
{
    [TestMethod]
    public void AddATaskToDBAndFile_ParseAndOperate_RemovedFromDBCorrectlyWithNoExceptionsOrWarnings()
    {
        TaskInfo task = new("Task 1", Difficulty.Easy, MinNumberOfTasks);
        CreateTasksInDB(task);
        CreateAndParseTasksInCSVFile(task);

        Operate();

        AssertReaderAndOperator(null, null);
        AssertTasks();
    }

    [TestMethod]
    public void AddATaskToDBAndOneToFileWithADifferentName_ParseAndOperate_NotRemovedFromDBAndGetAWarningAndNoExceptions()
    {
        TaskInfo task1 = new("Task 1", Difficulty.Easy, MinNumberOfTasks);
        TaskInfo task2 = new("Task 2", Difficulty.Easy, MinNumberOfTasks);
        CreateTasksInDB(task1);
        CreateAndParseTasksInCSVFile(task2);

        Operate();

        AssertReaderAndOperator(null, null, typeof(TaskDoesNotExistWarning));
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

        AssertReaderAndOperator(null, null, typeof(TaskDoesNotExistWarning));
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

        AssertReaderAndOperator(null, null, typeof(NotEnoughTasksToDeleteWarning));
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

        AssertReaderAndOperator(null, null);
        AssertTasks(task2);
    }

    private void CreateTasksInDB(params TaskInfo[] tasks)
    {
        foreach (TaskInfo task in tasks)
        {
            DataWorkerBefore.BingoTasks.CreateMany(task.Name, task.Difficulty, task.Amount);
        }
        DataWorkerBefore.SaveChanges();
    }
}