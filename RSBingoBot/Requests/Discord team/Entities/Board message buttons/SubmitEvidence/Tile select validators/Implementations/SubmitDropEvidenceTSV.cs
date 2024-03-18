// <copyright file="SubmitDropEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;

public class SubmitDropEvidenceTSV : TileSelectValidator<Tile, User, SubmitDropEvidenceDP>, ISubmitDropEvidenceTSV
{
    private readonly ITileCanRecieveDropsTSV tileCanRecieveDrops;
    private readonly IUserHasTheOnlyPendingDropsTSV userHasTheOnlyPendingDrops;

    public SubmitDropEvidenceTSV(IUserHasTheOnlyPendingDropsTSV otherUsersDropEvidenceSubmission,
        ITileCanRecieveDropsTSV tileDropEvidenceSubmission, SubmitDropEvidenceDP parser) :
        base(parser)
    {
        this.userHasTheOnlyPendingDrops = otherUsersDropEvidenceSubmission;
        this.tileCanRecieveDrops = tileDropEvidenceSubmission;
    }

    protected override bool Validate(SubmitDropEvidenceDP data) =>
        tileCanRecieveDrops.Validate(data.Tile) &&
        userHasTheOnlyPendingDrops.Validate(data.Tile, data.User);
}