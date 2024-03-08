// <copyright file="ScoringTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSBingo_Common;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingo_Framework.Scoring;
using static RSBingo_Framework.Records.BingoTaskRecord;
using static RSBingo_Framework.Records.TileRecord;

[TestClass]
public class ScoringTests : MockDBBaseTestClass
{
    private const string testTeamName = "Test";

    private IDataWorker dataWorker = null!;
    private Team team = null!;
    private TeamScore teamScore = null!;
    private BingoTask easyTaskOne = null!;
    private BingoTask easyTaskTwo = null!;
    private BingoTask mediumTaskOne = null!;
    private static int easyTileScore = 2;
    private static int mediumTileScore = 3;
    private static int bonusForRowCompletion = 5;
    private static int bonusForColumnCompletion = 10;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        TileValues.Initialise(bonusForRowCompletion, bonusForColumnCompletion);
        TileValues.SetDifficultyValue(Difficulty.Easy, easyTileScore);
        TileValues.SetDifficultyValue(Difficulty.Medium, mediumTileScore);
    }

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
        dataWorker = CreateDW();

        team = MockDBSetup.Add_Team(dataWorker, testTeamName);
        teamScore = new(team);

        easyTaskOne = MockDBSetup.Add_BingoTask(dataWorker, "Test1", Difficulty.Easy);
        easyTaskTwo = MockDBSetup.Add_BingoTask(dataWorker, "Test2", Difficulty.Easy);
        mediumTaskOne = MockDBSetup.Add_BingoTask(dataWorker, "Test3", Difficulty.Medium);

        dataWorker.SaveChanges();
    }

    [TestMethod]
    public void AddNoTiles_ScoreIsCorrect()
    {
        AssertScore(0);
    }

    [TestMethod]
    public void AddATile_MarkAsComplete_ScoreIsCorrect()
    {
        AddTile(easyTaskOne, 0, true);

        AssertScore(easyTileScore);
    }

    [TestMethod]
    public void AddTwoTilesWithTheSameDifficulty_MarkAsComplete_ScoreIsCorrect()
    {
        AddTile(easyTaskOne, 0, true);
        AddTile(easyTaskTwo, 1, true);

        AssertScore(easyTileScore * 2);
    }

    [TestMethod]
    public void AddTilesWithDifferentDifficulty_MarkAsComplete_ScoreIsCorrect()
    {
        AddTile(easyTaskOne, 0, true);
        AddTile(mediumTaskOne, 1, true);

        AssertScore(easyTileScore + mediumTileScore);
    }

    [TestMethod]
    public void AddTwoTilesWith_MarkOneAsComplete_ScoreIsCorrect()
    {
        AddTile(easyTaskOne, 0, false);
        AddTile(mediumTaskOne, 1, true);

        AssertScore(mediumTileScore);
    }

    [TestMethod]
    public void AddATile_MarkAsCompleteThenIncomplete_ScoreIsCorrect()
    {
        AddTile(easyTaskOne, 0, true);
        teamScore.Calculate();

        SetTileIncomplete(0);

        AssertScore(0);
    }

    [TestMethod]
    public void AddTilesInARow_MarkAsComplete_ScoreIsCorrect()
    {
        for (int i = 0; i < General.TilesPerRow; i++)
        {
            AddTile(easyTaskOne, i, true);
        }

        AssertScore(easyTileScore * General.TilesPerRow + bonusForRowCompletion);
    }

    [TestMethod]
    public void AddTilesInAColumn_MarkAsComplete_ScoreIsCorrect()
    {
        for (int i = 0; i < General.TilesPerColumn; i++)
        {
            AddTile(easyTaskOne, i * General.TilesPerRow, true);
        }

        AssertScore(easyTileScore * General.TilesPerColumn + bonusForColumnCompletion);
    }

    [TestMethod]
    public void AddTilesInARow_MarkAsCompleteThenMarkOneTileAsIncomplete_ScoreIsCorrect()
    {
        for (int i = 0; i < General.TilesPerRow; i++)
        {
            AddTile(easyTaskOne, i, true);
        }
        teamScore.Calculate();

        SetTileIncomplete(0);

        AssertScore(easyTileScore * General.TilesPerRow - easyTileScore);
    }

    [TestMethod]
    public void AddTilesInTwoRows_MarkAsComplete_ScoreIsCorrect()
    {
        for (int i = 0; i < General.TilesPerRow * 2; i++)
        {
            AddTile(easyTaskOne, i, true);
        }

        AssertScore((easyTileScore * General.TilesPerRow + bonusForRowCompletion) * 2);
    }

    [TestMethod]
    public void AddTilesInTwoRows_MarkAsCompleteThenMarkOneTileAsIncomplete_ScoreIsCorrect()
    {
        for (int i = 0; i < General.TilesPerRow * 2; i++)
        {
            AddTile(easyTaskOne, i, true);
        }
        teamScore.Calculate();

        SetTileIncomplete(0);

        AssertScore(easyTileScore * General.TilesPerRow * 2 - easyTileScore + bonusForRowCompletion);
    }

    private void AddTile(BingoTask task, int boardIndex, bool isComplete)
    {
        var completeStatus = isComplete ? CompleteStatus.Yes : CompleteStatus.No;
        MockDBSetup.Add_Tile(dataWorker, team, task, boardIndex, VerifiedStatus.Yes, completeStatus);
    }

    private void AssertScore(int expected)
    {
        teamScore.Calculate();
        Assert.AreEqual(expected, teamScore.Score);
    }

    private void SetTileIncomplete(int boardIndex)
    {
        team.Tiles.First(t => t.BoardIndex == boardIndex).SetCompleteStatus(CompleteStatus.No);
        dataWorker.SaveChanges();
    }
}