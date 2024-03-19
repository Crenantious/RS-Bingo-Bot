// <copyright file="IUserHasTheOnlyPendingDropsDP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DataParsers;

using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.EvidenceRecord;

public interface IUserHasTheOnlyPendingDropsDP : IDataParser<Tile, User>
{
    /// <summary>
    /// All <see cref="EvidenceStatus.PendingReview"/> <see cref="EvidenceType.Drop"/>
    /// <see cref="Models.Evidence"/> for the <see cref="Tile"/> the was submitted by the <see cref="User"/>.
    /// </summary>
    public IEnumerable<Evidence> Evidence { get; }
}