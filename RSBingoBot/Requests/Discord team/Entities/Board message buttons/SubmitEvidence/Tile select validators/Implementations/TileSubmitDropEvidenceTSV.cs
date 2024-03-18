// <copyright file="SubmitDropEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

public class TileSubmitDropEvidenceTSV : TileSelectValidator<Tile, TileSubmitDropEvidenceDP>, ITileSubmitDropEvidenceTSV
{
    public TileSubmitDropEvidenceTSV(TileSubmitDropEvidenceDP parser) : base(parser)
    {

    }

    protected override bool Validate(TileSubmitDropEvidenceDP data) =>
        data.Tile.IsVerified() &&
        data.Tile.IsCompleteAsBool() is false;
}