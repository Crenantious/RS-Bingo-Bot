﻿// <copyright file="UserHasNoAcceptedVerificationEvidenceForTileTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.TileValidators;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;

public class UserHasNoAcceptedVerificationEvidenceForTileTSV : TileSelectValidator<Tile, User, IUserHasNoAcceptedVerificationEvidenceForTileDP>,
    IUserHasNoAcceptedVerificationEvidenceForTileTSV
{
    public override string ErrorMessage => "You already have accepted verification evidence for this tile.";

    public UserHasNoAcceptedVerificationEvidenceForTileTSV(IUserHasNoAcceptedVerificationEvidenceForTileDP parser) : base(parser)
    {

    }

    protected override bool Validate(IUserHasNoAcceptedVerificationEvidenceForTileDP data) =>
        data.Evidence.Count() == 0;
}