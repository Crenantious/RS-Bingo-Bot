﻿// <copyright file="CompleteNextTileButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers.Testing;

using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingo_Framework.Scoring;

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
        if (Team!.Tiles.FirstOrDefault(t => t.IsCompleteAsBool() is false) is Tile tile)
        {
            tile.SetCompleteStatus(TileRecord.CompleteStatus.Yes);
            Team.UpdateScore(tile);
            return;
        }

        await args.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
            .WithContent("There are no incomplete tiles."));
    }
}