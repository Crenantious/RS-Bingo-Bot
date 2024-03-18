// <copyright file="SubmitDropEvidenceDP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DataParsers;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

public class SubmitVerificationEvidenceDP : IDataParser<Tile, User>
{
    public IEnumerable<Evidence> Evidence { get; private set; } = null!;

    public void Parse(Tile tile, User user)
    {
        Evidence = tile.Evidence
            .GetUserEvidence(user.DiscordUserId)
            .GetVerificationEvidence()
            .GetAcceptedEvidence();
    }
}