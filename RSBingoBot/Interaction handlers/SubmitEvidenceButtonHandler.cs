// <copyright file="SubmitEvidenceButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using static RSBingo_Framework.Records.EvidenceRecord;
using static RSBingo_Common.General;

// TODO: JR - disable the button if the team's tiles are being changed.
// Likewise, disable the change tiles button if this button is being interacted with.
// TODO: JR - split this into two different buttons; one for evidence and one for drops.
// TODO: JR - handle a user removing a tile from heir board after there being evidence submitted for it.
// Likely delete the evidence (and add a warning about it) as that causes the least problems.
/// <summary>
/// Handles the interaction with the "Submit evidence" button in a team's board channel.
/// </summary>
public class SubmitEvidenceButtonHandler : SubmitImageForTileButtonHandler
{         
    protected override int TileSelectMaxOptions { get; } = MaxTilesOnABoard;

    protected override EvidenceRecord.EvidenceType EvidenceType => EvidenceType.TileVerification;

    protected override IEnumerable<Tile> GetTileSelectTiles() =>
        Team!.Tiles;
}