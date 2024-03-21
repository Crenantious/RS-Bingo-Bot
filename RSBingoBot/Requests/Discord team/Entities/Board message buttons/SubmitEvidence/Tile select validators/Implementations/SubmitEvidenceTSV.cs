// <copyright file="SubmitEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.EvidenceRecord;

public class SubmitEvidenceTSV : TileSelectValidator<Tile, User, EvidenceType, ISubmitEvidenceDP>, ISubmitEvidenceTSV
{
    private readonly ISubmitDropEvidenceTSV dropValidator;
    private readonly ISubmitVerificationEvidenceTSV verificationValidator;

    private string errorMessage = string.Empty;

    public override string ErrorMessage => errorMessage;

    public SubmitEvidenceTSV(ISubmitDropEvidenceTSV dropEvidence, ISubmitVerificationEvidenceTSV verificationEvidence,
        ISubmitEvidenceDP parser) : base(parser)
    {
        this.dropValidator = dropEvidence;
        this.verificationValidator = verificationEvidence;
    }

    protected override bool Validate(ISubmitEvidenceDP data) =>
        data.EvidenceType switch
        {
            EvidenceType.TileVerification => VerificationValidation(data),
            EvidenceType.Drop => DropValidation(data),
            _ => throw new ArgumentOutOfRangeException($"The given {nameof(Enum)} {nameof(EvidenceType)} is invalid."),
        };

    private bool VerificationValidation(ISubmitEvidenceDP data)
    {
        bool isValid = verificationValidator.Validate(data.Tile, data.User);
        errorMessage = verificationValidator.ErrorMessage;
        return isValid;
    }

    private bool DropValidation(ISubmitEvidenceDP data)
    {
        bool isValid = dropValidator.Validate(data.Tile, data.User);
        errorMessage = dropValidator.ErrorMessage;
        return isValid;

    }
}