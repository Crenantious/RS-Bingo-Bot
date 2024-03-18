// <copyright file="UserHasNoAcceptedDropEvidenceDP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DataParsers;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

public class UserHasNoAcceptedDropEvidenceDP : IDataParser<User>
{
    public IEnumerable<Evidence> Evidence { get; private set; } = null!;

    public void Parse(User user)
    {
        Evidence = user.Evidence.GetDropEvidence()
                                .GetAcceptedEvidence();
    }
}