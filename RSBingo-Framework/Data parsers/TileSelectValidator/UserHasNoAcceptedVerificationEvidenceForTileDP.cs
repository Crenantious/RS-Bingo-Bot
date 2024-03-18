// <copyright file="UserHasNoAcceptedVerificationEvidenceForTileDP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DataParsers;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

public class UserHasNoAcceptedVerificationEvidenceForTileDP : IDataParser<Tile, User>
{
    /// <summary>
    /// Accepted verification evidence that the <see cref="User"/> has submitted for the <see cref="Tile"/>.
    /// </summary>
    public IEnumerable<Evidence> Evidence { get; private set; } = null!;

    public void Parse(Tile tile, User user)
    {
        Evidence = tile.Evidence
            .GetUserEvidence(user.DiscordUserId)
            .GetVerificationEvidence()
            .GetAcceptedEvidence();
    }
}