// <copyright file="SubmitEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.EvidenceRecord;

public class SubmitEvidenceTSV : ISubmitEvidenceTSV
{
    private readonly IDropEvidenceTSV dropEvidence;
    private readonly IVerificationEvidenceTSV verificationEvidence;

    public SubmitEvidenceTSV(IDropEvidenceTSV dropEvidence, IVerificationEvidenceTSV verificationEvidence)
    {
        this.dropEvidence = dropEvidence;
        this.verificationEvidence = verificationEvidence;
    }

    public bool Validate(IEnumerable<Tile> tiles, User user, EvidenceType evidenceType) =>
        tiles.All(t => Validate(t, user, evidenceType));

    public bool Validate(Tile tile, User user, EvidenceType evidenceType) =>
        evidenceType switch
        {
            EvidenceType.TileVerification => verificationEvidence.Validate(tile, user),
            EvidenceType.Drop => dropEvidence.Validate(tile, user),
            _ => throw new ArgumentOutOfRangeException(),
        };
}