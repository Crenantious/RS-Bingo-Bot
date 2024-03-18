// <copyright file="UserHasNoAcceptedVerificationEvidenceForTileTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;

public class UserHasNoAcceptedVerificationEvidenceForTileTSV : TileSelectValidator<Tile, User, UserHasNoAcceptedVerificationEvidenceForTileDP>,
    IUserHasNoAcceptedVerificationEvidenceForTileTSV
{
    public UserHasNoAcceptedVerificationEvidenceForTileTSV(UserHasNoAcceptedVerificationEvidenceForTileDP parser) : base(parser)
    {

    }

    protected override bool Validate(UserHasNoAcceptedVerificationEvidenceForTileDP data) =>
        data.Evidence.Count() == 0;
}