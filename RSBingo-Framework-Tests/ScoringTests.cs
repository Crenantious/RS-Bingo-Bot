// <copyright file="ScoringTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingoBot.Scoring;
using static RSBingo_Framework.Records.BingoTaskRecord;
using RSBingo_Framework.Records;

[TestClass]
public class ScoringTests : MockDBBaseTestClass
{
    private const string testTeamName = "Test";

    private static IDataWorker dataWorkerBefore = null!;
    private static IDataWorker dataWorkerAfter = null!;
    private static Team team = null!;
    private static BingoTask easyTaskOne = null!;
    private static BingoTask easyTaskTwo = null!;
    private static BingoTask mediumTaskOne = null!;
    private static Dictionary<Difficulty, int> pointsForDifficulty = new() {
        {Difficulty.Easy, 1 },
        {Difficulty.Medium, 2 },
        {Difficulty.Hard, 3 } };

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
        dataWorkerBefore = CreateDW();
        dataWorkerAfter = CreateDW();

        team = MockDBSetup.Add_Team(dataWorkerBefore, testTeamName);
        Scoring.CreateTeamScore(team);

        easyTaskOne = MockDBSetup.Add_BingoTask(dataWorkerBefore, "Test1", Difficulty.Easy);
        easyTaskTwo = MockDBSetup.Add_BingoTask(dataWorkerBefore, "Test2", Difficulty.Easy);
        mediumTaskOne = MockDBSetup.Add_BingoTask(dataWorkerBefore, "Test2", Difficulty.Medium);

        dataWorkerBefore.SaveChanges();
    }

    [TestMethod]
    public void AddATiles_MarkAsComplete_ScoreIsCorrect()
    {
        MockDBSetup.Add_Tile(dataWorkerBefore, team, easyTaskOne);
        Scoring.SetUpAsMock(dataWorkerBefore, pointsForDifficulty, new List<BonusPoints>());

        Tile tile = team.Tiles.ElementAt(0);
        tile.SetCompleteStatus(TileRecord.CompleteStatus.Yes);
        Scoring.UpdateTeamScore(tile);

        Assert.AreEqual(1, Scoring.GetTeamScore(team));
    }

    [TestMethod]
    public void AddTwoTiles_MarkOneAsComplete_ScoreIsCorrect()
    {
        MockDBSetup.Add_Tile(dataWorkerBefore, team, easyTaskOne);
        MockDBSetup.Add_Tile(dataWorkerBefore, team, easyTaskTwo);
        Scoring.SetUpAsMock(dataWorkerBefore, pointsForDifficulty, new List<BonusPoints>());

        Tile tile = team.Tiles.ElementAt(0);
        tile.SetCompleteStatus(TileRecord.CompleteStatus.Yes);
        Scoring.UpdateTeamScore(tile);

        Assert.AreEqual(1, Scoring.GetTeamScore(team));
    }

    [TestMethod]
    public void AddATile_MarkAsCompleteThenIncomplete_ScoreIsCorrect()
    {
        MockDBSetup.Add_Tile(dataWorkerBefore, team, easyTaskOne);
        Scoring.SetUpAsMock(dataWorkerBefore, pointsForDifficulty, new List<BonusPoints>());

        Tile tile = team.Tiles.ElementAt(0);
        tile.SetCompleteStatus(TileRecord.CompleteStatus.Yes);
        Scoring.UpdateTeamScore(tile);
        tile.SetCompleteStatus(TileRecord.CompleteStatus.No);
        Scoring.UpdateTeamScore(tile);

        Assert.AreEqual(0, Scoring.GetTeamScore(team));
    }

    [TestMethod]
    public void AddTilesWithDifferentDifficulty_MarkThemAsComplete_ScoreIsCorrect()
    {
        MockDBSetup.Add_Tile(dataWorkerBefore, team, easyTaskOne);
        MockDBSetup.Add_Tile(dataWorkerBefore, team, mediumTaskOne);
        Scoring.SetUpAsMock(dataWorkerBefore, pointsForDifficulty, new List<BonusPoints>());

        Tile easyTile = team.Tiles.ElementAt(0);
        Tile mediumTile = team.Tiles.ElementAt(1);
        easyTile.SetCompleteStatus(TileRecord.CompleteStatus.Yes);
        mediumTile.SetCompleteStatus(TileRecord.CompleteStatus.Yes);
        Scoring.UpdateTeamScore(easyTile);
        Scoring.UpdateTeamScore(mediumTile);

        Assert.AreEqual(3, Scoring.GetTeamScore(team));
    }

    [TestMethod]
    public void AddATileWithBonusPoints_MarkAsComplete_ScoreIsCorrect()
    {
        MockDBSetup.Add_Tile(dataWorkerBefore, team, easyTaskOne);
        List<BonusPoints> bonusPoints = new List<BonusPoints>() { new(new HashSet<int>() { 0 }, 5) };
        Scoring.SetUpAsMock(dataWorkerBefore, pointsForDifficulty, bonusPoints);

        Tile easyTileOne = team.Tiles.ElementAt(0);
        easyTileOne.SetCompleteStatus(TileRecord.CompleteStatus.Yes);
        Scoring.UpdateTeamScore(easyTileOne);

        Assert.AreEqual(6, Scoring.GetTeamScore(team));
    }

    [TestMethod]
    public void AddATileWithBonusPoints_MarkAsCompleteThenIncomplete_ScoreIsCorrect()
    {
        MockDBSetup.Add_Tile(dataWorkerBefore, team, easyTaskOne);
        List<BonusPoints> bonusPoints = new List<BonusPoints>() { new(new HashSet<int>() { 0 }, 5) };
        Scoring.SetUpAsMock(dataWorkerBefore, pointsForDifficulty, bonusPoints);

        Tile easyTileOne = team.Tiles.ElementAt(0);
        easyTileOne.SetCompleteStatus(TileRecord.CompleteStatus.Yes);
        Scoring.UpdateTeamScore(easyTileOne);
        easyTileOne.SetCompleteStatus(TileRecord.CompleteStatus.No);
        Scoring.UpdateTeamScore(easyTileOne);

        Assert.AreEqual(0, Scoring.GetTeamScore(team));
    }

    [TestMethod]
    public void AddTilesAndSomeWithBonusPoints_MarkBonusPointsTilesAsComplete_ScoreIsCorrect()
    {
        MockDBSetup.Add_Tile(dataWorkerBefore, team, easyTaskOne);
        MockDBSetup.Add_Tile(dataWorkerBefore, team, easyTaskTwo);
        List<BonusPoints> bonusPoints = new List<BonusPoints>() { new(new HashSet<int>() { 0 }, 5) };
        Scoring.SetUpAsMock(dataWorkerBefore, pointsForDifficulty, bonusPoints);

        Tile easyTileOne = team.Tiles.ElementAt(0);
        easyTileOne.SetCompleteStatus(TileRecord.CompleteStatus.Yes);
        Scoring.UpdateTeamScore(easyTileOne);

        Assert.AreEqual(6, Scoring.GetTeamScore(team));
    }

    [TestMethod]
    public void AddTilesAndSomeWithBonusPoints_MarkNonBonusPointsTilesAsComplete_ScoreIsCorrect()
    {
        MockDBSetup.Add_Tile(dataWorkerBefore, team, easyTaskOne);
        MockDBSetup.Add_Tile(dataWorkerBefore, team, easyTaskTwo);
        List<BonusPoints> bonusPoints = new List<BonusPoints>() { new(new HashSet<int>() { 0 }, 5) };
        Scoring.SetUpAsMock(dataWorkerBefore, pointsForDifficulty, bonusPoints);

        Tile easyTileTwo = team.Tiles.ElementAt(1);
        easyTileTwo.SetCompleteStatus(TileRecord.CompleteStatus.Yes);
        Scoring.UpdateTeamScore(easyTileTwo);

        Assert.AreEqual(1, Scoring.GetTeamScore(team));
    }
}