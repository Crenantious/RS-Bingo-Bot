// <copyright file="CompleteNextTileButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers.Testing;

using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingo_Framework.Scoring;
using static RSBingoBot.MessageUtilities;

/// <summary>
/// Handles the interaction with the "Complete next tile" button in a team's board channel.
/// </summary>
public class CompleteNextTileButtonHandler : ComponentInteractionHandler
{
    /// <inheritdoc/>
    protected override bool ContinueWithNullUser { get { return false; } }

    /// <inheritdoc/>
    protected override bool CreateAutoResponse { get { return true; } }

    /// <inheritdoc/>
    public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info)
    {
        await base.InitialiseAsync(args, info);
        await RunRequest(args);
        await ConcludeInteraction();
    }

    private async Task RunRequest(ComponentInteractionCreateEventArgs args)
    {
        if (Team!.Tiles.FirstOrDefault(t => t.IsCompleteAsBool() is false) is Tile tile)
        {
            tile.SetCompleteStatus(TileRecord.CompleteStatus.Yes);
            Team.UpdateScore(tile, DataWorker);
            DataWorker.SaveChanges();

            await EditResponse(args, "The next tile as been marked as completed.", true);
            return;
        }

        await EditResponse(args, "There are no incomplete tiles.", true);
    }
}