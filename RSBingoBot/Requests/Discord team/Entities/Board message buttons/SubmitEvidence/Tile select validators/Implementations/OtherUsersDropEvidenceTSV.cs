// <copyright file="OtherUsersDropEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;

public class OtherUsersDropEvidenceTSV : TileSelectValidator<Tile, User, OtherUsersDropEvidenceDP>, IOtherUsersDropEvidenceTSV
{
    public OtherUsersDropEvidenceTSV(OtherUsersDropEvidenceDP parser) : base(parser)
    {

    }

    protected override bool Validate(OtherUsersDropEvidenceDP data) =>
        data.DropEvidence.Count() == 0;
}