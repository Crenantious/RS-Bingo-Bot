// <copyright file="SubmitVerificationEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;

public class SubmitVerificationEvidenceTSV : TileSelectValidator<Tile, User, ISubmitVerificationEvidenceDP>, ISubmitVerificationEvidenceTSV
{
    private readonly IUserHasNoAcceptedVerificationEvidenceForTileTSV userHasNoAcceptedVerificationEvidenceForTile;

    public SubmitVerificationEvidenceTSV(IUserHasNoAcceptedVerificationEvidenceForTileTSV userHasNoAcceptedVerificationEvidenceForTile,
        ISubmitVerificationEvidenceDP parser) : base(parser)
    {
        this.userHasNoAcceptedVerificationEvidenceForTile = userHasNoAcceptedVerificationEvidenceForTile;
    }

    protected override bool Validate(ISubmitVerificationEvidenceDP data) =>
        userHasNoAcceptedVerificationEvidenceForTile.Validate(data.Tile, data.User);
}