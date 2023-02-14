// <copyright file="Scoring.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Scoring;

using RSBingo_Framework.Models;
using RSBingo_Framework.Interfaces;
using static RSBingo_Framework.DAL.DataFactory;
using static RSBingo_Common.General;
using static RSBingo_Framework.Records.BingoTaskRecord;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public static class Scoring
{
    public static int PointsForEasyTile { get; private set; }
    public static int PointsForMediumTile { get; private set; }
    public static int PointsForHardTile { get; private set; }

    public static Dictionary<int, List<BonusPoints>> BoardIndexToBonusPoints { get; private set; } = null!;

    public static Dictionary<Difficulty, int> PointsForDifficulty { get; private set; } = null!;

    private static HashSet<int>[] rowBoardIndexes = GetRowBoardIndexes();
    private static HashSet<int>[] columnBoardIndexes = GetColumnBoardIndexes();
    private static HashSet<int> easyBoardIndexes = new(rowBoardIndexes[0].Concat(rowBoardIndexes[1]));
    private static HashSet<int> mediumBoardIndexes = new(rowBoardIndexes[2].Concat(rowBoardIndexes[3]));
    private static HashSet<int> hardBoardIndexes = new(rowBoardIndexes[4]);

    // TODO: JR - Add diagonal BonusPoints

    private static BonusPoints[] rowBonusPoints = null!;
    private static BonusPoints[] columnBonusPoints = null!;
    private static BonusPoints easyBonusPoints = null!;
    private static BonusPoints mediumBonusPoints = null!;
    private static BonusPoints hardBonusPoints = null!;
    private static IEnumerable<BonusPoints> allBonusPoints = null!;

    private static bool IsInitialised = false;

    /// <summary>
    /// Sets up the <see cref="BonusPoints"/>, <see cref="TeamScore"/> and <see cref="TeamScore"/>s.
    /// </summary>
    public static void SetUp(ScoringConfig scoringConfig)
    {
        if (IsInitialised) { return; }

        PointsForEasyTile = scoringConfig.PointsForEasyTile;
        PointsForMediumTile = scoringConfig.PointsForMediumTile;
        PointsForHardTile = scoringConfig.PointsForHardTile;

        rowBonusPoints = CreateMultipleBonusPoints(rowBoardIndexes, scoringConfig.BonusPointsForRow);
        columnBonusPoints = CreateMultipleBonusPoints(columnBoardIndexes, scoringConfig.BonusPointsForColumn);
        easyBonusPoints = new(easyBoardIndexes, scoringConfig.BonusPointsForEasyCompletion);
        mediumBonusPoints = new(mediumBoardIndexes, scoringConfig.BonusPointsForMediumCompletion);
        hardBonusPoints = new(hardBoardIndexes, scoringConfig.BonusPointsForHardCompletion);

        allBonusPoints = new List<BonusPoints>(rowBonusPoints.Concat(columnBonusPoints)) { easyBonusPoints, mediumBonusPoints, hardBonusPoints };

        BoardIndexToBonusPoints = MapBoardIndexesToBonusPoints(allBonusPoints);

        PointsForDifficulty = new()
        {
            {Difficulty.Easy, PointsForEasyTile },
            {Difficulty.Medium, PointsForMediumTile },
            {Difficulty.Hard, PointsForHardTile },
        };

        IsInitialised = true;
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
}