// <copyright file="ISubmitDropEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.TileValidators;

using RSBingo_Framework.Models;

public interface ISubmitDropEvidenceTSV
{
    public string ErrorMessage { get; }

    /// <returns>If the <paramref name="user"/> is allowed to submit drop evidence for the <paramref name="tile"/>.</returns>
    public bool Validate(Tile tile, User user);
}