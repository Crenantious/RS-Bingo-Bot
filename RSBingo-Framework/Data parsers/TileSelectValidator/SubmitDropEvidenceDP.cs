// <copyright file="SubmitDropEvidenceDP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DataParsers;

using RSBingo_Framework.Models;

public class SubmitDropEvidenceDP : IDataParser<Tile, User>
{
    public Tile Tile { get; private set; } = null!;
    public User User { get; private set; } = null!;

    public void Parse(Tile tile, User user)
    {
        Tile = tile;
        User = user;
    }
}