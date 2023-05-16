// <copyright file="IHasScore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Interfaces;

using RSBingo_Framework.Models;
using RSBingo_Framework.Scoring;

public interface IHasScore
{
    public int Score { get; }

    public TeamScore TeamScore { get; }
}