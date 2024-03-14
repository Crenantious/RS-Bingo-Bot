// <copyright file="UserVerificationEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using static RSBingo_Framework.Records.EvidenceRecord;

public class UserVerificationEvidenceTSV : IUserVerificationEvidenceTSV
{
    public bool Validate(Tile tile, User user) =>
        user.Evidence.Any(e => tile.RowId == e.TileId &&
                               e.IsType(EvidenceType.TileVerification) &&
                               e.HasStatus(EvidenceStatus.Accepted));
}