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
using RSBingoBot.Interaction_handlers;
using RSBingoBot.Requests;
using System.Threading;
using FluentResults;
using RSBingoBot.DiscordEntities;
using RSBingoBot.Factories;
using RSBingoBot.DiscordEntities.Messages;

/// <summary>
/// Handles the Interaction with the "View evidence" button in a team's board channel.
/// </summary>
internal class ViewEvidenceButtonHandler : InteractionHandler<ViewEvidenceButtonRequest, Result>
{
    private const string InitialResponseMessagePrefix = "{0} Select a tile to view its evidence.";
    private const string NoTilesFoundError = "You have not submitted evidence for any tiles.";

    private readonly string tileSelectCustomId = Guid.NewGuid().ToString();

    private Message response = null!;
    private Tile? selectedTile = null;
    private SelectComponent tileSelect = null!;
    private DiscordButtonComponent closeButton = null!;
    private User User = null!;

    /// <inheritdoc/>
    public override async Task<Result> Handle(ViewEvidenceButtonRequest request, CancellationToken cancellationToken)
    {
        await base.Handle(request, cancellationToken);
        User = request.User;

        closeButton = ButtonFactory.GetClose(new(this));
        CreateTileSelect();
        response = GetResponseMessage();

        return Result.Ok();
    }

    private Message GetResponseMessage() =>
        new Message()
            .WithContent(InitialResponseMessagePrefix.FormatConst(Interaction.User.Mention))
            .AddComponents(tileSelect.discordComponent!)
            .AddComponents(closeButton);

    private void CreateTileSelect()
    {
        tileSelect = new SelectComponent("Select a tile", TileSelectItemSelected, TileSelectPageSelected, GetPageName);
        tileSelect.SelectOptions = GetTileSelectOptions();
        tileSelect.Build();
    }

    private List<SelectComponentOption> GetTileSelectOptions()
    {
        var tileSelectOptions = new List<SelectComponentOption>();
        foreach (Tile tile in User.Team.Tiles)
        {
            Evidence? evidence = tile.GetEvidence(DataWorker, User!.DiscordUserId);
            if (evidence is null) { continue; }

            DiscordEmoji? discordEmoji = BingoBotCommon.GetEvidenceStatusEmoji(evidence);
            DiscordComponentEmoji? emoji = discordEmoji is null ? null : new DiscordComponentEmoji(discordEmoji);
            tileSelectOptions.Add(new SelectComponentItem(tile.Task.Name, tile, null, selectedTile == tile, emoji));
        }

        return tileSelectOptions;
    }

    private string GetPageName(IEnumerable<SelectComponentOption> options) =>
        $"{options.ElementAt(0).label} - {options.ElementAt(options.Count() - 1).label}";

    private async Task TileSelectPageSelected(InteractionCreateEventArgs args) =>
        await UpdateOriginalResponse(Interaction);

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
        await Interaction.CreateFollowupMessageAsync(builder);
    }

    private async Task CancelButtonInteraction(DiscordClient client, ComponentInteractionCreateEventArgs args) =>
        await Conclude();
}