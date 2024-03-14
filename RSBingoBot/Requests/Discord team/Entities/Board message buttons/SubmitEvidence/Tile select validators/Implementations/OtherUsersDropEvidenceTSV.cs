// <copyright file="OtherUsersDropEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using static RSBingo_Framework.Records.EvidenceRecord;

public class OtherUsersDropEvidenceTSV : IOtherUsersDropEvidenceTSV
{
    public bool Validate(Tile tile, User user)
    {
        Evidence? evidence = GetPendingOrAcceptedDropEvidence(tile);

        if (evidence is null)
        {
            return true;
        }

        if (evidence.HasStatus(EvidenceStatus.PendingReview) &&
            evidence.DiscordUserId == evidence.User.DiscordUserId)
        {
            return true;
        }

        return false;
    }

    private Evidence? GetPendingOrAcceptedDropEvidence(Tile tile) =>
        tile.Evidence.First(e => e.IsType(EvidenceType.Drop) &&
                                 (e.HasStatus(EvidenceStatus.PendingReview) || e.HasStatus(EvidenceStatus.Accepted)));
}