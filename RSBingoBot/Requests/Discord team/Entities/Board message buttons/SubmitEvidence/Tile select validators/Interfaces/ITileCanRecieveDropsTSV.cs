// <copyright file="ISubmitDropEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.EvidenceRecord;

public interface ITileCanRecieveDropsTSV
{
    /// <returns>If the <paramref name="tile"/> is allowed to have <see cref="EvidenceType.Drop"/>
    /// <see cref="Evidence"/> submitted for it.</returns>
    public bool Validate(Tile tile);
}