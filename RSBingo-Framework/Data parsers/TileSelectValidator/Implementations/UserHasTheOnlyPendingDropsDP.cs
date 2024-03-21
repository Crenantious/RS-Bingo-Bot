// <copyright file="UserHasTheOnlyPendingDropsDP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DataParsers;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using static RSBingo_Framework.Records.EvidenceRecord;

public class UserHasTheOnlyPendingDropsDP : IUserHasTheOnlyPendingDropsDP
{
    public IEnumerable<Evidence> Evidence { get; private set; } = null!;

    public void Parse(Tile tile, User user)
    {
        Evidence = tile.Evidence
            .GetDropEvidence()
            .GetPendingEvidence()
            .GetOtherUsersEvidence(user.DiscordUserId);
    }
}