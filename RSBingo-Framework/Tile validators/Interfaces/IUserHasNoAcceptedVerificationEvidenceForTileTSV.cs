// <copyright file="IUserHasNoAcceptedVerificationEvidenceForTileTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.TileValidators;

using RSBingo_Framework.Models;

public interface IUserHasNoAcceptedVerificationEvidenceForTileTSV
{
    public string ErrorMessage { get; }

    public bool Validate(Tile tile, User user);
}