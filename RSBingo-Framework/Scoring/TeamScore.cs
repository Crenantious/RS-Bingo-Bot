// <copyright file="TeamScore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Scoring;

using RSBingo_Framework.Models;

public class TeamScore
{
    private readonly Team team;

    /// <summary>
    /// The most recently calculated score. Defaults to 0.
    /// </summary>
    public int Score { get; private set; } = 0;

    public TeamScore(Team team)
    {
        this.team = team;
    }

    public void Calculate()
    {
        int score = 0;

        IEnumerable<TilesValue> tilesScores = TileValues.GetTileValues(team.Tiles);

        foreach (Tile tile in team.Tiles)
        {
            foreach (TilesValue tilesScore in tilesScores)
            {
                score += tilesScore.Get(tile.BoardIndex);
            }
        }

        Score = score;
    }
}