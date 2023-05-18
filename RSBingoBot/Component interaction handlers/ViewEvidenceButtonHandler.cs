// <copyright file="ViewEvidenceButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingo_Framework.Exceptions;
using RSBingoBot.Discord_event_handlers;
using RSBingoBot.Component_interaction_handlers.Select_Component;

/// <summary>
/// Handles the interaction with the "View evidence" button in a team's board channel.
/// </summary>
public class ViewEvidenceButtonHandler : ComponentInteractionHandler
{
    private const string initialResponseMessagePrefix = "Select a tile to view its evidence.";

    private readonly string tileSelectCustomId = Guid.NewGuid().ToString();

    private Tile? selectedTile = null;
    private SelectComponent tileSelect = null!;
    private DiscordButtonComponent closeButton = null!;

    /// <inheritdoc/>
    protected override bool ContinueWithNullUser => false;

    /// <inheritdoc/>
    protected override bool CreateAutoResponse => true;

    /// <inheritdoc/>
    protected override bool IsAutoResponseEphemeral => false;

    /// <inheritdoc/>
    protected override bool AllowInteractionWithAnotherComponent => true;

    /// <inheritdoc/>
    public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info)
    {
        await base.InitialiseAsync(args, info);

        initialResponseMessagePrefix.FormatConst(args.User.Mention, initialResponseMessagePrefix);
        closeButton = new DiscordButtonComponent(ButtonStyle.Primary, Guid.NewGuid().ToString(), "Close");
        CreateTileSelect();

        var builder = new DiscordWebhookBuilder()
            .WithContent(initialResponseMessagePrefix)
            .AddComponents(tileSelect.DiscordComponent)
            .AddComponents(closeButton);

        await args.Interaction.EditOriginalResponseAsync(builder);
        MessagesForCleanup.Add(await args.Interaction.GetOriginalResponseAsync());

        SubscribeComponent(new ComponentInteractionDEH.Constraints(user: args.User, customId: tileSelect.CustomId),
            tileSelect.OnInteraction, true);
        SubscribeComponent(new ComponentInteractionDEH.Constraints(user: args.User, customId: closeButton.CustomId),
            CancelButtonInteraction, true);
    }

    private void CreateTileSelect()
    {
        var tileSelectOptions = new List<SelectComponentOption>();
        foreach (Tile tile in Team!.Tiles)
        {
            Evidence? evidence = tile.GetEvidence(DataWorker, User!.DiscordUserId);
            if (evidence is null) { continue; }

            DiscordEmoji? discordEmoji = BingoBotCommon.GetEvidenceStatusEmoji(evidence);
            DiscordComponentEmoji? emoji = discordEmoji is null ? null : new DiscordComponentEmoji(discordEmoji);
            tileSelectOptions.Add(new SelectComponentItem(tile.Task.Name, tile, null, selectedTile == tile, emoji));
        }

        tileSelect = new SelectComponent(tileSelectCustomId, "Select tiles", TileSelectItemSelected);
        tileSelect.SelectOptions = tileSelectOptions;
        tileSelect.Build();
    }

    private async Task TileSelectItemSelected(ComponentInteractionCreateEventArgs args)
    {
        selectedTile = (Tile)tileSelect.SelectedItems[0].value!;
        Evidence? evidence = selectedTile.GetEvidence(DataWorker, args.User.Id);
        if (evidence is null)
        {
            // TODO: JR - test this once the CommandController has been converted to using requests.
            throw new ComponentInteractionHandlerException(
                $"Evidence can no longer be found for the tile {selectedTile.Task.Name}", args,
                false, ComponentInteractionHandlerException.ErrorResponseType.CreateFollowUpResponse, true);
        }

        await PostEvidence(evidence);
    }

    private async Task PostEvidence(Evidence evidence)
    {
        var builder = new DiscordFollowupMessageBuilder()
            .WithContent($"Evidence submitted for tile {evidence.Tile.Task.Name} {evidence.Url}")
            .AsEphemeral();
        await CurrentInteractionArgs.Interaction.CreateFollowupMessageAsync(builder);
    }

    private async Task CancelButtonInteraction(DiscordClient client, ComponentInteractionCreateEventArgs args) =>
        await ConcludeInteraction();
}