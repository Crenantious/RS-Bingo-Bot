// <copyright file="TeamScore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Scoring;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingoBot.Exceptions;
using static RSBingo_Framework.Records.BingoTaskRecord;

internal class TeamScore
{
    /// <summary>
    /// Maps tile board indexes to all <see cref="BonusPoints"/> that require them for completing.
    /// </summary>
    public static Dictionary<int, List<BonusPoints>> BoardIndexToBonusPoints { get; set; } = new();

    /// <summary>
    /// The points awarded for completing a tile with a given difficulty.
    /// </summary>
    public static Dictionary<Difficulty, int> PointsForDifficulty { get; set; } = new();

    public int Score { get; private set; } = 0;

    /// <summary>
    /// Updates the team's score.
    /// </summary>
    /// <param name="tile">The tile who's completion status changed, triggering the need to update the team's score.</param>
    /// <exception cref="TileDifficultyPointValueNotSetException"/>
    public void Update(Tile tile)
    {
        if (PointsForDifficulty.ContainsKey(tile.Task.GetDifficutyAsDifficulty()) is false)
        {
            throw new TileDifficultyPointValueNotSetException($"Point value has not been set for {tile.Task.GetDifficutyAsDifficulty().ToString()} tiles.");
        }

        UpdateScoreFromDifficulty(tile);

        if (BoardIndexToBonusPoints.ContainsKey(tile.BoardIndex) is false) { return; }
        UpdateBonusPointsAndScore(tile);
    }

    private void UpdateScoreFromDifficulty(Tile tile)
    {
        // This assumes the tile's complete status has changed
        int sign = tile.IsCompleteAsBool() ? 1 : -1;
        Score += sign * PointsForDifficulty[tile.Task.GetDifficutyAsDifficulty()];
    }

    private void UpdateBonusPointsAndScore(Tile tile)
    {
        foreach (BonusPoints bonusPoints in BoardIndexToBonusPoints[tile.BoardIndex])
        {
            bool bonusAchievedPreviously = bonusPoints.HasBonusBeenAchieved(tile.Team);
            bonusPoints.Update(tile);
            bool bonusAchievedCurrently = bonusPoints.HasBonusBeenAchieved(tile.Team);

            if (bonusAchievedCurrently != bonusAchievedPreviously)
            {
                if (bonusAchievedCurrently) { Score += bonusPoints.BonusValue; }
                else { Score -= bonusPoints.BonusValue; }
            }
        }
    }
}