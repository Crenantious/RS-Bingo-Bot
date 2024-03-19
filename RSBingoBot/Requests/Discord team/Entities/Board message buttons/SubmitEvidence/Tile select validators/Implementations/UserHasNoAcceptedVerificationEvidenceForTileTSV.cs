// <copyright file="UserHasNoAcceptedVerificationEvidenceForTileTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;

public class UserHasNoAcceptedVerificationEvidenceForTileTSV : TileSelectValidator<Tile, User, IUserHasNoAcceptedVerificationEvidenceForTileDP>,
    IUserHasNoAcceptedVerificationEvidenceForTileTSV
{
    public UserHasNoAcceptedVerificationEvidenceForTileTSV(IUserHasNoAcceptedVerificationEvidenceForTileDP parser) : base(parser)
    {

    }

    protected override bool Validate(IUserHasNoAcceptedVerificationEvidenceForTileDP data) =>
        data.Evidence.Count() == 0;
}