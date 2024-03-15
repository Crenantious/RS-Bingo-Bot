// <copyright file="IUserHasNoAcceptedDropEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.EvidenceRecord;

public interface IUserHasNoAcceptedDropEvidenceTSV
{
    /// <returns>If the user has no <see cref="EvidenceStatus.Accepted"/>
    /// <see cref="EvidenceType.Drop"/> <see cref="Evidence"/>.</returns>
    public bool Validate(User user);
}