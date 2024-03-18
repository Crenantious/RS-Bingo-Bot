
// <copyright file="UserHasNoAcceptedDropEvidenceDP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DataParsers;

using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.EvidenceRecord;

public class SubmitEvidenceDP : IDataParser<Tile, User, EvidenceType>
{
    public Tile Tile { get; private set; } = null!;
    public User User { get; private set; } = null!;
    public EvidenceType EvidenceType { get; private set; }

    public void Parse(Tile tile, User user, EvidenceType evidenceType)
    {
        Tile = tile;
        User = user;
        EvidenceType = evidenceType;
    }
}