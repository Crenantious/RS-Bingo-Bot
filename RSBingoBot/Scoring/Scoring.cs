// <copyright file="Scoring.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Scoring;

using RSBingo_Framework.Models;
using RSBingo_Framework.Interfaces;
using static RSBingo_Framework.DAL.DataFactory;
using static RSBingo_Common.General;
using static RSBingo_Framework.Records.BingoTaskRecord;

public static class Scoring
{
    private const int bonusPointsPerRow = 1;
    private const int bonusPointsPerColumn = 1;

    private static HashSet<int> easyBonusBoardIndexes = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private static HashSet<int> mediumBonusBoardIndexes = new() { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
    private static HashSet<int> hardBonusBoardIndexes = new() { 20, 21, 22, 23, 24 };

    private static BonusPoints easyBonusPoints = new(easyBonusBoardIndexes, 1);
    private static BonusPoints mediumBonusPoints = new(mediumBonusBoardIndexes, 2);
    private static BonusPoints hardBonusPoints = new(hardBonusBoardIndexes, 3);

    private static Dictionary<Difficulty, int> pointsForDifficulty = new() {
        {Difficulty.Easy, 1 },
        {Difficulty.Medium, 2 },
        {Difficulty.Hard, 3 } };
    private static Dictionary<Team, TeamScore> teamScores = new();
    private static IEnumerable<BonusPoints> allBonusPoints;
    private static IDataWorker dataWorker;

    /// <summary>
    /// Sets up the <see cref="BonusPoints"/>, <see cref="TeamScore"/> and <see cref="TeamScore"/>s.
    /// </summary>
    public static void SetUp()
    {
        dataWorker = CreateDataWorker();
        IEnumerable<BonusPoints> allBonusPoints = CreateAllBonusPoints();
        CommonSetUp(pointsForDifficulty, allBonusPoints);
    }

    /// <summary>
    /// Sets up the <see cref="TeamScore"/> and <see cref="TeamScore"/>s.
    /// </summary>
    public static void SetUpAsMock(IDataWorker dataWorker, Dictionary<Difficulty, int> pointsForDifficulty,
        IEnumerable<BonusPoints> bonusPoints)
    {
        Scoring.dataWorker = dataWorker;
        Scoring.pointsForDifficulty = pointsForDifficulty;
        CommonSetUp(pointsForDifficulty, bonusPoints);
    }

    public static void CreateTeamScore(Team team)
    {
        if (teamScores.ContainsKey(team) is false)
        {
            teamScores.Add(team, new TeamScore());
        }
    }

    public static int GetTeamScore(Team team) =>
        teamScores[team].Score;

    private static void CommonSetUp(Dictionary<Difficulty, int> pointsForDifficulty, IEnumerable<BonusPoints> allBonusPoints)
    {
        Scoring.allBonusPoints = allBonusPoints;
        TeamScore.BoardIndexToBonusPoints = MapBoardIndexesToBonusPoints(allBonusPoints);
        TeamScore.PointsForDifficulty = pointsForDifficulty;
        CreateTeamScores();
    }

    private static IEnumerable<BonusPoints> CreateAllBonusPoints()
    {
        BonusPoints[] rowAndColumnBonusPoints = CreateRowAndColumnBonusPoints(GetRowBoardIndexes(), GetColumnBoardIndexes());
        BonusPoints[] otherBonusPoints = new BonusPoints[] { easyBonusPoints, mediumBonusPoints, hardBonusPoints };
        return rowAndColumnBonusPoints.Concat(otherBonusPoints);
    }

    private static IEnumerable<HashSet<int>> GetRowBoardIndexes()
    {
        HashSet<int>[] rowBonusBoardIndexes = new HashSet<int>[TilesPerColumn];

        for (int i = 0; i < MaxTilesOnABoard; i++)
        {
            rowBonusBoardIndexes[i / TilesPerRow].Add(i);
        }

        return rowBonusBoardIndexes;
    }

    private static IEnumerable<HashSet<int>> GetColumnBoardIndexes()
    {
        HashSet<int>[] columnBonusBoardIndexes = new HashSet<int>[TilesPerRow];

        for (int i = 0; i < MaxTilesOnABoard; i++)
        {
            columnBonusBoardIndexes[i % TilesPerColumn].Add(i);
        }

        return columnBonusBoardIndexes;
    }

    private static BonusPoints[] CreateRowAndColumnBonusPoints(IEnumerable<HashSet<int>> rowBoardIndexes,
        IEnumerable<HashSet<int>> columnBoardIndexes)
    {
        BonusPoints[] bonusPoints = new BonusPoints[rowBoardIndexes.Count() + columnBoardIndexes.Count()];

        foreach (HashSet<int> boardIndexes in rowBoardIndexes)
        {
            bonusPoints[^1] = new(boardIndexes, bonusPointsPerRow);
        }

        foreach (HashSet<int> boardIndexes in columnBoardIndexes)
        {
            bonusPoints[^1] = new(boardIndexes, bonusPointsPerColumn);
        }

        return bonusPoints;
    }

    private static Dictionary<int, List<BonusPoints>> MapBoardIndexesToBonusPoints(IEnumerable<BonusPoints> bonusPointsEnumerable)
    {
        Dictionary<int, List<BonusPoints>> bonusPointsMap = new();

        foreach (BonusPoints bonusPoints in bonusPointsEnumerable)
        {
            foreach (int boardIndex in bonusPoints.BoardIndexes)
            {
                bonusPointsMap.TryAdd(boardIndex, new List<BonusPoints>());
                if (bonusPointsMap[boardIndex].Contains(bonusPoints) is false)
                {
                    bonusPointsMap[boardIndex].Add(bonusPoints);
                }
            }
        }

        return bonusPointsMap;
    }

    private static void CreateTeamScores()
    {
        foreach (Team team in dataWorker.Teams.GetAll())
        {
            CreateTeamScore(team);
        }
    }

    public static void UpdateTeamScore(Tile tileThatChanged) =>
        teamScores[tileThatChanged.Team].Update(tileThatChanged);
}