// <copyright file="TeamScore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Scoring;

using RSBingo_Framework.Exceptions;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using static RSBingo_Framework.Records.BingoTaskRecord;
using static RSBingo_Framework.Scoring.Scoring;

public class TeamScore
{
    public int Score { get; private set; } = 0;

    public delegate Task AsyncEventData(TeamScore teamScore);

    public static event AsyncEventData ScoreUpdatedEventAsync;

    /// <summary>
    /// Updates the team's score.
    /// </summary>
    /// <param name="tile">The tile who's completion status changed, triggering the need to recalculate the team's score.</param>
    /// <exception cref="TileDifficultyPointValueNotSetException"/>
    public void Update(Tile tile)
    {
        ValidateTileDifficulty(tile);
        UpdateScoreFromDifficulty(tile);

        if (BoardIndexToBonusPoints.ContainsKey(tile.BoardIndex)) { UpdateBonusPointsAndScore(tile); }

        ScoreUpdatedEventAsync?.Invoke(this);
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

    private static void ValidateTileDifficulty(Tile tile)
    {
        if (PointsForDifficulty.ContainsKey(tile.Task.GetDifficutyAsDifficulty()) is false)
        {
            throw new TileDifficultyPointValueNotSetException($"Point value has not been set for {tile.Task.GetDifficutyAsDifficulty().ToString()} tiles.");
        }
    }
}