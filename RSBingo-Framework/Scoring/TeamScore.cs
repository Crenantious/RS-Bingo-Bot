// <copyright file="TeamScore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Scoring;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

public class TeamScore
{
    /// <summary>
    /// The most recently calculated score. Defaults to 0.
    /// </summary>
    public int Score { get; private set; } = 0;

    public void Calculate(IEnumerable<Tile> tiles)
    {
        int score = 0;

        List<TilesValue> tilesValues = TileValues.GetTileValues(tiles).ToList();
        tiles = tiles.Where(t => t.IsCompleteAsBool());

        foreach (Tile tile in tiles)
        {
            foreach (TilesValue tilesScore in tilesValues)
            {
                score += tilesScore.Get(tile.BoardIndex);
            }
        }

        Score = score;
    }
}