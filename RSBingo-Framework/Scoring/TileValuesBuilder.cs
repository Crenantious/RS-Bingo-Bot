// <copyright file="TileValuesBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Scoring;

using Microsoft.Extensions.Configuration;
using static RSBingo_Framework.Records.BingoTaskRecord;

internal class TileValuesBuilder
{
    public void Build(IConfiguration config)
    {
        var scoring = config.GetSection("scoring");

        int row = GetValue(scoring, "row");
        int column = GetValue(scoring, "column");
        TileValues.Initialise(row, column);

        foreach (Difficulty difficulty in GetDifficulties())
        {
            TileValues.SetDifficultyValue(difficulty, GetValue(scoring, "difficulty:" + difficulty.ToString()));
        }
    }

    private static int GetValue(IConfigurationSection scoring, string path)
    {
        string? value = scoring.GetSection(path).Value;
        if (value is null)
        {
            throw new Exception();
        }

        if (int.TryParse(value, out int a))
        {
            return a;
        }

        throw new Exception();
    }

    private static IEnumerable<Difficulty> GetDifficulties() =>
        Enum.GetValues<Difficulty>()
            .Where(d => d != Difficulty.Undefined);
}