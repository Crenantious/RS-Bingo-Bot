// <copyright file="SubmitEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.EvidenceRecord;

public class SubmitEvidenceTSV : TileSelectValidator<Tile, User, EvidenceType, SubmitEvidenceDP>, ISubmitEvidenceTSV
{
    private readonly ISubmitDropEvidenceTSV dropValidator;
    private readonly ISubmitVerificationEvidenceTSV verificationValidator;

    public SubmitEvidenceTSV(ISubmitDropEvidenceTSV dropEvidence, ISubmitVerificationEvidenceTSV verificationEvidence,
        SubmitEvidenceDP parser) : base(parser)
    {
        this.dropValidator = dropEvidence;
        this.verificationValidator = verificationEvidence;
    }

    protected override bool Validate(SubmitEvidenceDP data) =>
        data.EvidenceType switch
        {
            EvidenceType.TileVerification => verificationValidator.Validate(data.Tile, data.User),
            EvidenceType.Drop => dropValidator.Validate(data.Tile, data.User),
            _ => throw new ArgumentOutOfRangeException($"The given {nameof(Enum)} {nameof(EvidenceType)} is invalid."),
        };
}