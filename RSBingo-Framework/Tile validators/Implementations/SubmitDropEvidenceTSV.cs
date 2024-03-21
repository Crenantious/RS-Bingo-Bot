// <copyright file="SubmitDropEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.TileValidators;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;

public class SubmitDropEvidenceTSV : TileSelectValidator<Tile, User, ISubmitDropEvidenceDP>, ISubmitDropEvidenceTSV
{
    private readonly ITileCanRecieveDropsTSV tileCanRecieveDrops;
    private readonly IUserHasTheOnlyPendingDropsTSV userHasTheOnlyPendingDrops;

    private string errorMessage = string.Empty;

    public override string ErrorMessage => errorMessage;

    public SubmitDropEvidenceTSV(ITileCanRecieveDropsTSV tileDropEvidenceSubmission,
        IUserHasTheOnlyPendingDropsTSV otherUsersDropEvidenceSubmission, ISubmitDropEvidenceDP parser) :
        base(parser)
    {
        this.tileCanRecieveDrops = tileDropEvidenceSubmission;
        this.userHasTheOnlyPendingDrops = otherUsersDropEvidenceSubmission;
    }

    protected override bool Validate(ISubmitDropEvidenceDP data)
    {
        if (tileCanRecieveDrops.Validate(data.Tile) is false)
        {
            errorMessage = tileCanRecieveDrops.ErrorMessage;
            return false;
        }

        errorMessage = userHasTheOnlyPendingDrops.ErrorMessage;
        return userHasTheOnlyPendingDrops.Validate(data.Tile, data.User);
    }
}