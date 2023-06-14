// <copyright file="ClearTeamsEvidenceButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers.Testing;

using DSharpPlus.EventArgs;
using RSBingo_Framework.Models;
using static RSBingoBot.MessageUtilities;

/// <summary>
/// Handles the interaction with the "Clear evidence" button in a team's board channel.
/// </summary>
public class ClearTeamsEvidenceButtonHandler : ComponentInteractionHandler
{
    /// <inheritdoc/>
    protected override bool ContinueWithNullUser { get { return false; } }
    
    /// <inheritdoc/>
    protected override bool CreateAutoResponse { get { return true; } }

    /// <inheritdoc/>
    public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info)
    {
        await base.InitialiseAsync(args, info);

        foreach (Tile tile in Team!.Tiles)
        {
            foreach (Evidence evidence in tile.Evidence)
            {
                DataWorker.Evidence.Remove(evidence);
            }
        }

        await Respond(args.Interaction, "All evidence for this team has been successfully deleted.", true);
        await ConcludeInteraction();
    }
}