// <copyright file="SubmitDropEvidenceDP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DataParsers;

using RSBingo_Framework.Models;

public class TileSubmitDropEvidenceDP : IDataParser<Tile>
{
    public Tile Tile { get; private set; } = null!;

    public void Parse(Tile tile)
    {
        Tile = tile;
    }
}