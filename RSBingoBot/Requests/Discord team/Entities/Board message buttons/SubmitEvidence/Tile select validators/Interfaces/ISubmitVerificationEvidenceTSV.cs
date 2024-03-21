// <copyright file="ISubmitVerificationEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.EvidenceRecord;

public interface ISubmitVerificationEvidenceTSV
{
    public string ErrorMessage { get; }

    /// <returns>If the <paramref name="user"/> is allowed to submit
    /// <see cref="EvidenceType.TileVerification"/> <see cref="Evidence"/>
    /// for <paramref name="tile"/>.</returns>
    public bool Validate(Tile tile, User user);
}