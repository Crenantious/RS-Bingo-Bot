// <copyright file="SubmitDropEvidenceDP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DataParsers;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

public class OtherUsersDropEvidenceDP : IDataParser<Tile, User>
{
    public IEnumerable<Evidence> DropEvidence { get; private set; } = null!;

    public void Parse(Tile tile, User user)
    {
        DropEvidence = tile.Evidence.GetDropEvidence()
            .GetOtherUsersEvidence(user.DiscordUserId)
            .GetNonRejectedEvidence();
    }
}