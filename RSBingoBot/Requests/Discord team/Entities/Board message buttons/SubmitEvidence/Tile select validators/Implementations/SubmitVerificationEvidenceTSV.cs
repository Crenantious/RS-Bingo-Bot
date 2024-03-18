// <copyright file="SubmitVerificationEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;

public class SubmitVerificationEvidenceTSV : TileSelectValidator<Tile, User, SubmitVerificationEvidenceDP>, ISubmitVerificationEvidenceTSV
{
    public SubmitVerificationEvidenceTSV(SubmitVerificationEvidenceDP parser) : base(parser)
    {

    }

    protected override bool Validate(SubmitVerificationEvidenceDP data) =>
        data.Evidence.Count() == 0;
}