// <copyright file="OtherUsersDropEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using static RSBingo_Framework.Records.EvidenceRecord;

public class OtherUsersDropEvidenceTSV : IOtherUsersDropEvidenceTSV
{
    public bool Validate(IEnumerable<Evidence> dropEvidence, ulong userId) =>
        dropEvidence.GetOtherUsersEvidence(userId)
            .GetNonRejectedEvidence()
            .Any();
}