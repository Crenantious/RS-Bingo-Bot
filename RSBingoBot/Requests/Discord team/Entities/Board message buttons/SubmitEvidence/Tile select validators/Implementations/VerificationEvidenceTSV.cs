// <copyright file="VerificationEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using static RSBingo_Framework.Records.EvidenceRecord;

public class VerificationEvidenceTSV : IVerificationEvidenceTSV
{
    public bool Validate(Tile tile, User user)
    {
        Evidence? evidence = GetUserVerificationEvidence(tile, user);
        return evidence is null || evidence.IsAccepted() is false;
    }

    private Evidence? GetUserVerificationEvidence(Tile tile, User user) =>
        user.Evidence.FirstOrDefault(e => e.TileId == tile.RowId &&
                                          e.IsType(EvidenceType.TileVerification));
}