// <copyright file="TileVerificationTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;

public class TileVerificationTSV : ITileVerificationTSV
{
    private readonly IUserVerificationEvidenceTSV verificationEvidence;

    public TileVerificationTSV(IUserVerificationEvidenceTSV verificationEvidence)
    {
        this.verificationEvidence = verificationEvidence;
    }

    public bool Validate(Tile tile) =>
        tile.Team.Users.All(u => verificationEvidence.Validate(tile, u));
}