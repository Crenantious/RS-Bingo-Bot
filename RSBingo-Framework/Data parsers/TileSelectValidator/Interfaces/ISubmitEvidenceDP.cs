// <copyright file="ISubmitEvidenceDP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DataParsers;

using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.EvidenceRecord;

public interface ISubmitEvidenceDP : IDataParser<Tile, User, EvidenceType>
{
    public Tile Tile { get; }
    public User User { get; }
    public EvidenceType EvidenceType { get; }
}