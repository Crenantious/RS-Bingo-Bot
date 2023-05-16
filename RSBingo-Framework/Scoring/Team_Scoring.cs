// <copyright file="Team_Scoring.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Models;

using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Scoring;

public partial class Team : IHasScore
{
    private TeamScore? teamScore;

    public int Score { get; set; }

    public TeamScore TeamScore => teamScore ??= new();
}