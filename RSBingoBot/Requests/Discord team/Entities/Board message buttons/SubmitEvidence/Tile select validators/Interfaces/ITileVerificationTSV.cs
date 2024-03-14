// <copyright file="ITileVerificationTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;

public interface ITileVerificationTSV
{
    public bool Validate(Tile tile);
}