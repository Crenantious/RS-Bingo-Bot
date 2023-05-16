// <copyright file="TeamScore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Scoring;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingo_Framework.Exceptions;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using static RSBingo_Framework.Scoring.Scoring;

public class TeamScore
{
    // TODO: JR - fire an event when a score gets updated to reduce coupling with endpoints and the leaderboard.
    // This needs to be done in a way that does not conflict with EF Core:
    // Updating the score on the Team record will not work as data workers will need to be created after the change and that is not guaranteed.
    // Also, it would require a data worker to be passed in here to be saved before the event is fired, which is not desirable.

    /// <summary>
    /// Updates the team's score.
    /// </summary>
    /// <param name="tile">The tile who's completion status changed, triggering the need to recalculate the team's score.</param>
    /// <exception cref="TileDifficultyPointValueNotSetException"/>
    public static void Update(Tile tile)
    {
        ValidateTileDifficulty(tile);
        UpdateScoreFromDifficulty(tile);

        // TODO: fix. Does not seem to give any bonus points.
        //if (BoardIndexToBonusPoints.ContainsKey(tile.BoardIndex)) { UpdateBonusPointsAndScore(tile); }
    }

    private static void UpdateScoreFromDifficulty(Tile tile)
    {
        // This assumes the tile's complete status has changed
        int sign = tile.IsCompleteAsBool() ? 1 : -1;
        tile.Team.Score += sign * PointsForDifficulty[tile.Task.GetDifficutyAsDifficulty()];
    }

    private static void UpdateBonusPointsAndScore(Tile tile)
    {
        foreach (BonusPoints bonusPoints in BoardIndexToBonusPoints[tile.BoardIndex])
        {
            bool bonusAchievedPreviously = bonusPoints.HasBonusBeenAchieved(tile.Team);
            bonusPoints.Update(tile);
            bool bonusAchievedCurrently = bonusPoints.HasBonusBeenAchieved(tile.Team);

            if (bonusAchievedCurrently != bonusAchievedPreviously)
            {
                if (bonusAchievedCurrently) { tile.Team.Score += bonusPoints.BonusValue; }
                else { tile.Team.Score -= bonusPoints.BonusValue; }
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