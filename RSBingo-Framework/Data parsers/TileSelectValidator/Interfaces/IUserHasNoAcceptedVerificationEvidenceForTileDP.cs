// <copyright file="IUserHasNoAcceptedVerificationEvidenceForTileDP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DataParsers;

using RSBingo_Framework.Models;

public interface IUserHasNoAcceptedVerificationEvidenceForTileDP : IDataParser<Tile, User>
{
    /// <summary>
    /// Accepted verification evidence that the <see cref="User"/> has submitted for the <see cref="Tile"/>.
    /// </summary>
    public IEnumerable<Evidence> Evidence { get; }
}