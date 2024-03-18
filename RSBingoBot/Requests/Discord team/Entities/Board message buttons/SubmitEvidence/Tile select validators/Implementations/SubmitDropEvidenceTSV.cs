// <copyright file="SubmitDropEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;

public class SubmitDropEvidenceTSV : TileSelectValidator<Tile, User, SubmitDropEvidenceDP>, ISubmitDropEvidenceTSV
{
    private readonly ITileSubmitDropEvidenceTSV tileDropEvidenceSubmission;
    private readonly IOtherUsersDropEvidenceTSV otherUsersDropEvidence;
    private readonly IUserHasNoAcceptedDropEvidenceTSV userHasNoAcceptedDropEvidence;

    public SubmitDropEvidenceTSV(IOtherUsersDropEvidenceTSV otherUsersDropEvidenceSubmission,
        ITileSubmitDropEvidenceTSV tileDropEvidenceSubmission, IUserHasNoAcceptedDropEvidenceTSV userHasNoAcceptedDropEvidence,
        SubmitDropEvidenceDP parser) : base(parser)
    {
        this.otherUsersDropEvidence = otherUsersDropEvidenceSubmission;
        this.tileDropEvidenceSubmission = tileDropEvidenceSubmission;
        this.userHasNoAcceptedDropEvidence = userHasNoAcceptedDropEvidence;
    }

    protected override bool Validate(SubmitDropEvidenceDP data) =>
        tileDropEvidenceSubmission.Validate(data.Tile) &&
        otherUsersDropEvidence.Validate(data.Tile, data.User) &&
        userHasNoAcceptedDropEvidence.Validate(data.User);
}