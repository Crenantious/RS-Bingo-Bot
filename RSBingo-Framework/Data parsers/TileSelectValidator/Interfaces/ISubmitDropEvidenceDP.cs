// <copyright file="ISubmitDropEvidenceDP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DataParsers;

using RSBingo_Framework.Models;

public interface ISubmitDropEvidenceDP : IDataParser<Tile, User>
{
    public Tile Tile { get; }
    public User User { get; }
}