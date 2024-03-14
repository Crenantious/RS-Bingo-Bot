// <copyright file="ITileVerificationTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;

public interface ITileVerificationTSV
{
    /// <returns>If the <paramref name="tile"/> has accepted verification evidence for each user of the team.</returns>
    public bool Validate(Tile tile);
}