// <copyright file="UserHasNoAcceptedDropEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using static RSBingo_Framework.Records.EvidenceRecord;

public class UserHasNoAcceptedDropEvidenceTSV : IUserHasNoAcceptedDropEvidenceTSV
{
    public bool Validate(User user) =>
        user.Evidence.GetDropEvidence()
            .GetAcceptedEvidence()
            .Count() == 0;
}