// <copyright file="SubmitDropEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

public class TileSubmitDropEvidenceTSV : ITileSubmitDropEvidenceTSV
{
    public bool Validate(Tile tile) =>
        tile.IsVerified() &&
        tile.IsCompleteAsBool() is false;
}