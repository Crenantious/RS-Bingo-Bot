// <copyright file="IDropEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;

public interface IDropEvidenceTSV
{
    /// <returns>If the <paramref name="user"/> is allowed to submit drop evidence for the <paramref name="tile"/>.</returns>
    public bool Validate(Tile tile, User user);
}