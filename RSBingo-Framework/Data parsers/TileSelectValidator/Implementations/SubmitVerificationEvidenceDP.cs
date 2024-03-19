// <copyright file="SubmitVerificationEvidenceDP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DataParsers;

using RSBingo_Framework.Models;

public class SubmitVerificationEvidenceDP : ISubmitVerificationEvidenceDP
{
    public Tile Tile { get; set; } = null!;
    public User User { get; set; } = null!;

    public void Parse(Tile tile, User user)
    {
        Tile = tile;
        User = user;
    }
}