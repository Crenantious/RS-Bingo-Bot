// <copyright file="SubmitVerificationEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.TileValidators;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;

public class SubmitVerificationEvidenceTSV : TileSelectValidator<Tile, User, ISubmitVerificationEvidenceDP>, ISubmitVerificationEvidenceTSV
{
    private readonly IUserHasNoAcceptedVerificationEvidenceForTileTSV userHasNoAcceptedVerificationEvidenceForTile;

    public override string ErrorMessage => userHasNoAcceptedVerificationEvidenceForTile.ErrorMessage;

    public SubmitVerificationEvidenceTSV(IUserHasNoAcceptedVerificationEvidenceForTileTSV userHasNoAcceptedVerificationEvidenceForTile,
        ISubmitVerificationEvidenceDP parser) : base(parser)
    {
        this.userHasNoAcceptedVerificationEvidenceForTile = userHasNoAcceptedVerificationEvidenceForTile;
    }

    protected override bool Validate(ISubmitVerificationEvidenceDP data) =>
        userHasNoAcceptedVerificationEvidenceForTile.Validate(data.Tile, data.User);
}