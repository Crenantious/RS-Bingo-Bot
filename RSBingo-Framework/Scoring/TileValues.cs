// <copyright file="TileValues.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Scoring;

using RSBingo_Common;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

// This is static because it needs the config, but can't get that until DI is setup, thus this cannot be
// registered as a service with DI.
public static class TileValues
{
    private static Dictionary<BingoTaskRecord.Difficulty, int> difficultyValues = new();

    private static IEnumerable<TilesValue> baseScores = Enumerable.Empty<TilesValue>();

    public static void Initialise(int rowValue, int columnValue)
    {
        if (rowValue != 0)
        {
            baseScores= baseScores.Concat(GetRowScores(rowValue));
        }

        if (columnValue != 0)
        {
            baseScores = baseScores.Concat(GetColumnScores(columnValue));
        }
    }

    public static void SetDifficultyValue(BingoTaskRecord.Difficulty difficulty, int value)
    {
        difficultyValues[difficulty] = value;
    }

    public static IEnumerable<TilesValue> GetTileValues(IEnumerable<Tile> tiles) =>
        baseScores.Concat(GetDifficultyScores(tiles));

    private static IEnumerable<TilesValue> GetDifficultyScores(IEnumerable<Tile> tiles) =>
        tiles.Select(t => new TilesValue(GetDifficultyValue(t), t.BoardIndex));

    private static int GetDifficultyValue(Tile tile) =>
        difficultyValues[tile.Task.GetDifficutyAsDifficulty()];

    private static List<TilesValue> GetRowScores(int value)
    {
        List<TilesValue> rows = new();
        int[] rowIndexes = new int[General.TilesPerColumn];

        for (int i = 0; i < General.TilesPerColumn; i++)
        {
            for (int j = 0; j < General.TilesPerRow; j++)
            {
                rowIndexes[j] = j + i * General.TilesPerRow;
            }
            rows.Add(new(value, rowIndexes));
        }

        return rows;
    }

    private static List<TilesValue> GetColumnScores(int value)
    {
        List<TilesValue> columns = new();
        int[] columnIndexes = new int[General.TilesPerRow];

        for (int i = 0; i < General.TilesPerRow; i++)
        {
            for (int j = 0; j < General.TilesPerColumn; j++)
            {
                columnIndexes[j] = i + j * General.TilesPerRow;
            }
            columns.Add(new(value, columnIndexes));
        }

        return columns;
    }
}