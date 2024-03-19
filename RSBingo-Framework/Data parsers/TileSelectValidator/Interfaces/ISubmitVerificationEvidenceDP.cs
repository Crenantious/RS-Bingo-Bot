// <copyright file="ISubmitVerificationEvidenceDP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DataParsers;

using RSBingo_Framework.Models;

public interface ISubmitVerificationEvidenceDP : IDataParser<Tile, User>
{
    public Tile Tile { get; set; }
    public User User { get; set; }
}