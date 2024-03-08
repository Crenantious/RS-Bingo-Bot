// <copyright file="TilesValue.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Scoring;

/// <summary>
/// Assigns a score value to a collection of tiles in order for the score to be awarded when all of those tiles are completed.
/// </summary>
public class TilesValue
{
    private HashSet<int> boardIndexes;
    public int Score { get; }

    /// <param name="boardIndexes">The indexes that must be completed for <paramref name="score"/> to be awarded</param>
    public TilesValue(int score, params int[] boardIndexes)
    {
        Score = score;
        this.boardIndexes = boardIndexes.ToHashSet();
    }

    /// <summary>
    /// Marks <paramref name="boardIndex"/> as complete for this instance.
    /// </summary>
    /// <returns>The <see cref="Score"/> if all required board indexes are complete, 0 otherwise.</returns>
    public int Get(int boardIndex)
    {
        if (boardIndexes.Contains(boardIndex) is false)
        {
            return 0;
        }

        boardIndexes.Remove(boardIndex);
        if (boardIndexes.Any() is false)
        {
            return Score;
        }

        return 0;
    }
}