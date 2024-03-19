// <copyright file="SubmitDropEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;

public class SubmitDropEvidenceTSV : TileSelectValidator<Tile, User, ISubmitDropEvidenceDP>, ISubmitDropEvidenceTSV
{
    private readonly ITileCanRecieveDropsTSV tileCanRecieveDrops;
    private readonly IUserHasTheOnlyPendingDropsTSV userHasTheOnlyPendingDrops;

    public SubmitDropEvidenceTSV(ITileCanRecieveDropsTSV tileDropEvidenceSubmission,
        IUserHasTheOnlyPendingDropsTSV otherUsersDropEvidenceSubmission, ISubmitDropEvidenceDP parser) :
        base(parser)
    {
        this.tileCanRecieveDrops = tileDropEvidenceSubmission;
        this.userHasTheOnlyPendingDrops = otherUsersDropEvidenceSubmission;
    }

    protected override bool Validate(ISubmitDropEvidenceDP data) =>
        tileCanRecieveDrops.Validate(data.Tile) &&
        userHasTheOnlyPendingDrops.Validate(data.Tile, data.User);
}