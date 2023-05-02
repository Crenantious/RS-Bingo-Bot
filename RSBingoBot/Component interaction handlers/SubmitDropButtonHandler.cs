// <copyright file="SubmitEvidenceButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using static RSBingo_Framework.Records.EvidenceRecord;

/// <summary>
/// Handles the interaction with the "Submit drop" button in a team's board channel.
/// </summary>
public class SubmitDropButtonHandler : SubmitImageForTileButtonHandler
{
    protected override int TileSelectMaxOptions => 1;

    protected override EvidenceType EvidenceType => EvidenceType.Drop;

    protected override IEnumerable<Tile> GetTileSelectTiles() =>
        Team!.Tiles.Where(t => t.IsCompleteAsBool() is false);

    private string GetTileNameForConfirmationMessage(Tile tile)
    {
        string tileName = tile.Task.Name;
        int indexOfTilesWithSameTaskNameOnBoard = Team!.Tiles
            .Where(t => t.Task.Name == tile.Task.Name)
            .ToList()
            .IndexOf(tile);

        if (indexOfTilesWithSameTaskNameOnBoard > 0)
        {
            tileName += $"({indexOfTilesWithSameTaskNameOnBoard + 1})";
        }
        return tileName;
    }
}