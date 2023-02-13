// <copyright file="MockDBSetup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using static RSBingo_Framework.DAL.DataFactory;
using static RSBingo_Framework.Records.BingoTaskRecord;

public class MockDBSetup
{
    public static void SetupDataFactory()
    {
        DataFactory.SetupDataFactory(true);

        TestCleanUpDB(nameof(MockDBSetup));
    }

    public static void TestCleanUpDB(TestContext testContext) { TestCleanUpDB(testContext.FullTestName()); }

    public static void TestInitializeDB(TestContext testContext) { TestInitializeDB(testContext.FullTestName()); }

    public static BingoTask Add_BingoTask(IDataWorker dataWorker, string name, Difficulty difficulty)
    {
        BingoTask bingoTask = dataWorker.BingoTasks.Create(name, difficulty);

        dataWorker.SaveChanges();
        return bingoTask;
    }

    public static Team Add_Team(IDataWorker dataWorker, string name)
    {
        Team team = dataWorker.Teams.Create(name, 0, 0);

        dataWorker.SaveChanges();
        return team;
    }

    private static void TestInitializeDB(string name)
    {
        IDataWorker dataWorker = CreateDataWorker(name);
        dataWorker.Context.Database.EnsureCreated();

        dataWorker.SaveChanges();
    }

    private static void TestCleanUpDB(string name)
    {
        IDataWorker dataWorker = CreateDataWorker(name);
        dataWorker.Context.Database.EnsureDeleted();

        dataWorker.SaveChanges();
    }
}
