// <copyright file="DropEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

public class DropEvidenceTSV : IDropEvidenceTSV
{
    private readonly ITileVerificationTSV tileVerification;
    private readonly IOtherUsersDropEvidenceTSV otherUsersDropEvidence;

    public DropEvidenceTSV(ITileVerificationTSV tileVerification, IOtherUsersDropEvidenceTSV otherUsersDropEvidence)
    {
        this.tileVerification = tileVerification;
        this.otherUsersDropEvidence = otherUsersDropEvidence;
    }

    public bool Validate(Tile tile, User user)
    {
        if (tile.IsCompleteAsBool() || IsTileVerified(tile) is false)
        {
            return false;
        }

        return otherUsersDropEvidence.Validate(tile, user);
    }

    private bool IsTileVerified(Tile tile) =>
        tileVerification.Validate(tile);
}