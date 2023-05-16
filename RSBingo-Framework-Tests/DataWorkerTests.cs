// <copyright file="DataWorkerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.BingoTaskRecord;
using static RSBingo_Framework.DAL.DataFactory;

[TestClass]
public class DataWorkerTests
{
    [TestMethod]
    public void a()
    {
        IDataWorker workerBefore = CreateDataWorker();
        BingoTask a = workerBefore.BingoTasks.Create("a", Difficulty.Easy);
        a.Name = "b";

        IDataWorker workerAfter = CreateDataWorker();
        Write(workerBefore, workerAfter, a);
    }
    [TestMethod]

    public void b()
    {
        IDataWorker workerBefore = CreateDataWorker();
        BingoTask a = workerBefore.BingoTasks.Create("a", Difficulty.Easy);
        a.Name = "b";
        workerBefore.SaveChanges();

        IDataWorker workerAfter = CreateDataWorker();
        Write(workerBefore, workerAfter, a);
    }
    [TestMethod]

    public void c()
    {
        IDataWorker workerBefore = CreateDataWorker();
        IDataWorker workerAfter = CreateDataWorker();
        BingoTask a = workerBefore.BingoTasks.Create("a", Difficulty.Easy);
        BingoTask b = workerAfter.BingoTasks.GetAll().ToArray()[^1];
        a.Name = "b";

        Write(workerBefore, workerAfter, a);
    }
    [TestMethod]

    public void d()
    {
        IDataWorker workerBefore = CreateDataWorker();
        IDataWorker workerAfter = CreateDataWorker();
        BingoTask a = workerBefore.BingoTasks.Create("a", Difficulty.Easy);
        a.Name = "b";
        workerBefore.SaveChanges();

        Write(workerBefore, workerAfter, a);
    }
    [TestMethod]
    private void Write(IDataWorker before, IDataWorker after, BingoTask task)
    {
        //Console.WriteLine($"{before.BingoTasks.CountAll().ToString()}, {after.BingoTasks.CountAll().ToString()}");
        var a = before.BingoTasks.GetAll().ToArray()[^1];
        var b = after.BingoTasks.GetAll().ToArray()[^1];
        Console.WriteLine(string.Join(", ", a.RowId, a.Name, b.RowId, b.Name));
    }
}