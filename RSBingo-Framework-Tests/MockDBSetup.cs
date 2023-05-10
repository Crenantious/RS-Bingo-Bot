// <copyright file="MockDBSetup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using System;
using static RSBingo_Framework.DAL.DataFactory;
using static RSBingo_Framework.Records.BingoTaskRecord;
using static RSBingo_Framework.Records.EvidenceRecord;
using static RSBingo_Framework.Records.TileRecord;

public class MockDBSetup
{
    public static void SetupDataFactory()
    {
        DataFactory.SetupDataFactory(true);

        TestCleanUpDB(nameof(MockDBSetup));
    }

    public static void TestCleanUpDB(TestContext testContext) { TestCleanUpDB(testContext.FullTestName()); }

    public static void TestInitializeDB(TestContext testContext) { TestInitializeDB(testContext.FullTestName()); }

    public static Team Add_Team(IDataWorker dataWorker, string name = "Test")
    {
        Team team = dataWorker.Teams.Create(name, 0, 0, 0, 0, 0, 0);

        dataWorker.SaveChanges();
        return team;
    }

    public static User Add_User(IDataWorker dataWorker, ulong discordId, Team team)
    {
        User user = dataWorker.Users.Create(discordId, team);

        dataWorker.SaveChanges();
        return user;
    }

    public static Tile Add_Tile(IDataWorker dataWorker, Team team, BingoTask bingoTask, int? boardIndex = null,
        VerifiedStatus verifiedStatus = VerifiedStatus.No)
    {
        int index = boardIndex ?? team.Tiles.Count;
        Tile tile = dataWorker.Tiles.Create(team, bingoTask, index, verifiedStatus);

        dataWorker.SaveChanges();
        return tile;
    }

    public static Evidence Add_Evidence(IDataWorker dataWorker, User user, Tile tile, string url = "",
        EvidenceType evidenceType = EvidenceType.TileVerification, ulong discordMessageId = 0)
    {
        Evidence evidence = dataWorker.Evidence.Create(user, tile, url, evidenceType, discordMessageId);

        dataWorker.SaveChanges();
        return evidence;
    }

    public static BingoTask Add_BingoTask(IDataWorker dataWorker, string name, Difficulty difficulty = Difficulty.Easy)
    {
        BingoTask bingoTask = dataWorker.BingoTasks.Create(name, difficulty);

        dataWorker.SaveChanges();
        return bingoTask;
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
    }
}
