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
    private static HashSet<int>[] rowBoardIndexes = GetRowBoardIndexes();
    private static HashSet<int>[] columnBoardIndexes = GetColumnBoardIndexes();
    private static HashSet<int> easyBoardIndexes = new(rowBoardIndexes[0].Concat(rowBoardIndexes[1]));
    private static HashSet<int> mediumBoardIndexes = new(rowBoardIndexes[2].Concat(rowBoardIndexes[3]));
    private static HashSet<int> hardBoardIndexes = new(rowBoardIndexes[4]);

    private static BonusPoints[] rowBonusPoints = CreateMultipleBonusPoints(rowBoardIndexes, 1);
    private static BonusPoints[] columnBonusPoints = CreateMultipleBonusPoints(columnBoardIndexes, 1);
    private static BonusPoints easyBonusPoints = new(easyBoardIndexes, 1);
    private static BonusPoints mediumBonusPoints = new(mediumBoardIndexes, 2);
    private static BonusPoints hardBonusPoints = new(hardBoardIndexes, 3);
    private static IEnumerable<BonusPoints> allBonusPoints = new List<BonusPoints>(rowBonusPoints.Concat(columnBonusPoints))
    { easyBonusPoints, mediumBonusPoints, hardBonusPoints };

    private static Dictionary<Difficulty, int> pointsForDifficulty = new() {
        {Difficulty.Easy, 1 },
        {Difficulty.Medium, 2 },
        {Difficulty.Hard, 3 } };
    private static Dictionary<Team, TeamScore> teamScores = new();
    private static IDataWorker dataWorker;

    /// <summary>
    /// Sets up the <see cref="BonusPoints"/>, <see cref="TeamScore"/> and <see cref="TeamScore"/>s.
    /// </summary>
    public static void SetUp()
    {
        dataWorker = CreateDataWorker();
        CommonSetUp(pointsForDifficulty, allBonusPoints);
    }

    /// <summary>
    /// Sets up the <see cref="TeamScore"/> and <see cref="TeamScore"/>s.
    /// </summary>
    public static void SetUpAsMock(IDataWorker dataWorker, Dictionary<Difficulty, int> pointsForDifficulty,
        IEnumerable<BonusPoints> bonusPoints)
    {
        Scoring.dataWorker = dataWorker;
        CommonSetUp(pointsForDifficulty, bonusPoints);
    }

    public static void AddTeam(Team team)
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

    private static BonusPoints[] CreateMultipleBonusPoints(IEnumerable<HashSet<int>> boardIndexes, int bonusValue)
    {
        BonusPoints[] bonusPoints = new BonusPoints[boardIndexes.Count()];
        foreach(HashSet<int> indexes in boardIndexes)
        {
            bonusPoints[^1] = new(indexes, bonusValue);
        }
        return bonusPoints;
    }

    private static HashSet<int>[] GetRowBoardIndexes()
    {
        HashSet<int>[] rowBonusBoardIndexes = new HashSet<int>[TilesPerColumn];

        for (int i = 0; i < MaxTilesOnABoard; i++)
        {
            if (rowBonusBoardIndexes[i / TilesPerRow] == default)
            {
                rowBonusBoardIndexes[i / TilesPerRow] = new HashSet<int>();
            }
            rowBonusBoardIndexes[i / TilesPerRow].Add(i);
        }

        return rowBonusBoardIndexes;
    }

    private static HashSet<int>[] GetColumnBoardIndexes()
    {
        HashSet<int>[] columnBonusBoardIndexes = new HashSet<int>[TilesPerRow];

        for (int i = 0; i < MaxTilesOnABoard; i++)
        {
            if (columnBonusBoardIndexes[i % TilesPerColumn] == default)
            {
                columnBonusBoardIndexes[i % TilesPerColumn] = new HashSet<int>();
            }
            columnBonusBoardIndexes[i % TilesPerColumn].Add(i);
        }

        return columnBonusBoardIndexes;
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
            AddTeam(team);
        }
    }

    public static void UpdateTeamScore(Tile tileThatChanged) =>
        teamScores[tileThatChanged.Team].Update(tileThatChanged);
}