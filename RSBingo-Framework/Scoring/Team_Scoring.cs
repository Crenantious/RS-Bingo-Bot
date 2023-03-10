// <copyright file="Team.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Models;

using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Scoring;

public partial class Team : IHasScore
{
    private TeamScore? teamScore;

    public int Score => TeamScore.Score;

    public TeamScore TeamScore => teamScore ??= new();

    public void UpdateScore(Tile tile) =>
        TeamScore.Update(tile);
}