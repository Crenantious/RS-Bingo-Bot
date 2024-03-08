// <copyright file="TileValues.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Scoring;

using RSBingo_Common;
using RSBingo_Common.DataStructures;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

// This is static because it needs the config, but can't get that until DI is setup, thus this cannot be
// registered as a service with DI.
public static class TileValues
{
    private static Dictionary<BingoTaskRecord.Difficulty, int> difficultyValues = new();

    private static int rowValue;
    private static int columnValue;
    private static Grid<int> boardIndexes = new(General.TilesPerRow, General.TilesPerColumn);

    public static void Initialise(int rowValue, int columnValue)
    {
        for (int i = 0; i < General.TilesPerColumn; i++)
        {
            int[] rowIndexes = new int[General.TilesPerRow];
            for (int j = 0; j < General.TilesPerRow; j++)
            {
                rowIndexes[j] = j + i * General.TilesPerRow;
            }
            boardIndexes.SetRow(i, rowIndexes);
        }

        TileValues.rowValue = rowValue;
        TileValues.columnValue = columnValue;
    }

    public static void SetDifficultyValue(BingoTaskRecord.Difficulty difficulty, int value)
    {
        difficultyValues[difficulty] = value;
    }

    public static IEnumerable<TilesValue> GetTileValues(IEnumerable<Tile> tiles) =>
        GetRowValues()
            .Concat(GetColumnValues())
            .Concat(GetDifficultyScores(tiles));

    private static IEnumerable<TilesValue> GetDifficultyScores(IEnumerable<Tile> tiles) =>
        tiles.Select(t => new TilesValue(GetDifficultyValue(t), t.BoardIndex));

    private static int GetDifficultyValue(Tile tile) =>
        difficultyValues[tile.Task.GetDifficutyAsDifficulty()];

    private static IEnumerable<TilesValue> GetRowValues() =>
        boardIndexes.Rows.Select(i => new TilesValue(rowValue, i));

    private static IEnumerable<TilesValue> GetColumnValues() =>
        boardIndexes.Columns.Select(i => new TilesValue(columnValue, i));
}