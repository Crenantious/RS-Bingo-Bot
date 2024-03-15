// <copyright file="SubmitDropEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

public class SubmitDropEvidenceTSV : ISubmitDropEvidenceTSV
{
    private readonly ITileSubmitDropEvidenceTSV tileDropEvidenceSubmission;
    private readonly IOtherUsersDropEvidenceTSV otherUsersDropEvidence;
    private readonly IUserHasNoAcceptedDropEvidenceTSV userHasNoAcceptedDropEvidence;

    public SubmitDropEvidenceTSV(IOtherUsersDropEvidenceTSV otherUsersDropEvidenceSubmission,
        ITileSubmitDropEvidenceTSV tileDropEvidenceSubmission, IUserHasNoAcceptedDropEvidenceTSV userHasNoAcceptedDropEvidence)
    {
        this.otherUsersDropEvidence = otherUsersDropEvidenceSubmission;
        this.tileDropEvidenceSubmission = tileDropEvidenceSubmission;
        this.userHasNoAcceptedDropEvidence = userHasNoAcceptedDropEvidence;
    }

    public bool Validate(Tile tile, User user)
    {
        if (tileDropEvidenceSubmission.Validate(tile) is false)
        {
            return false;
        }

        return otherUsersDropEvidence.Validate(tile.Evidence.GetDropEvidence(), user.DiscordUserId) &&
               userHasNoAcceptedDropEvidence.Validate(user);
    }
}