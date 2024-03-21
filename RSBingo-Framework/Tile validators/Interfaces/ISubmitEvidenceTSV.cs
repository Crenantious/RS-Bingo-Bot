// <copyright file="ISubmitEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.TileValidators;

using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.EvidenceRecord;

public interface ISubmitEvidenceTSV
{
    public string ErrorMessage { get; }

    public bool Validate(Tile tile, User user, EvidenceType evidenceType);
}