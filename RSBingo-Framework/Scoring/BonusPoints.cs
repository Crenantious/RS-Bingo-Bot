// <copyright file="BonusPoints.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Scoring;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

public class BonusPoints
{
    public int BonusValue { get; private set; }

    /// <summary>
    /// The board indexes of tiles on a team's board that are required to be completed to award the bonus points.
    /// </summary>
    public HashSet<int> BoardIndexes { get; private set; }

    private Dictionary<Team, bool> bonusAchieved = new();
    private Dictionary<Team, HashSet<int>> completedTiles = new();

    public BonusPoints(HashSet<int> boardIndexs, int bonusValue)
    {
        BoardIndexes = boardIndexs;
        this.BonusValue = bonusValue;
    }

    /// <summary>
    /// Updates the collection of tiles are completed and determine if the bonus has been achieved.
    /// </summary>
    /// <param name="tile">The tile who's completion status changed, triggering the need to update the team's score.</param>
    public void Update(Tile tile)
    {
        if (BoardIndexes.Contains(tile.BoardIndex) is false) { return; }

        if (completedTiles.ContainsKey(tile.Team) is false) { AddTeam(tile.Team); }

        if (tile.IsCompleteAsBool())
        {
            completedTiles[tile.Team].Add(tile.BoardIndex);
        }
        else if (completedTiles[tile.Team].Contains(tile.BoardIndex))
        {
            completedTiles[tile.Team].Remove(tile.BoardIndex);
        }

        bonusAchieved[tile.Team] = BoardIndexes.Count() == completedTiles[tile.Team].Count();
    }

    /// <summary>
    /// Gets whether or not the bonus has been achieved, rewarded by completing all tiles with board indexes in 
    /// <see cref="BoardIndexes"/>.
    /// </summary>
    public bool HasBonusBeenAchieved(Team team)
    {
        if (completedTiles.ContainsKey(team) is false) { AddTeam(team); }
        return bonusAchieved[team];
    }

    private void AddTeam(Team team)
    {
        completedTiles[team] = new HashSet<int>();
        bonusAchieved[team] = false;
    }
}