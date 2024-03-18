// <copyright file="ISubmitEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.EvidenceRecord;

public interface ISubmitEvidenceTSV
{
    public bool Validate(Tile tile, User user, EvidenceType evidenceType);
}