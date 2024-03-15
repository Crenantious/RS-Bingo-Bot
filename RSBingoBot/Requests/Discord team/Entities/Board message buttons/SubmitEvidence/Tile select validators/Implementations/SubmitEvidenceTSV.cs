// <copyright file="SubmitEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.EvidenceRecord;

public class SubmitEvidenceTSV : ISubmitEvidenceTSV
{
    private readonly ISubmitDropEvidenceTSV dropValidator;
    private readonly ISubmitVerificationEvidenceTSV verificationValidator;

    public SubmitEvidenceTSV(ISubmitDropEvidenceTSV dropEvidence, ISubmitVerificationEvidenceTSV verificationEvidence)
    {
        this.dropValidator = dropEvidence;
        this.verificationValidator = verificationEvidence;
    }

    public bool Validate(IEnumerable<Tile> tiles, User user, EvidenceType evidenceType) =>
        tiles.All(t => Validate(t, user, evidenceType));

    public bool Validate(Tile tile, User user, EvidenceType evidenceType) =>
        evidenceType switch
        {
            EvidenceType.TileVerification => verificationValidator.Validate(tile, user),
            EvidenceType.Drop => dropValidator.Validate(tile, user),
            _ => throw new ArgumentOutOfRangeException(),
        };
}