// <copyright file="ViewEvidenceButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using RSBingo_Framework.Exceptions;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingoBot.DiscordComponents;
using RSBingoBot.DiscordEntities;
using RSBingoBot.DiscordEntities.Messages;
using RSBingoBot.Factories;
using RSBingoBot.InteractionHandlers;
using RSBingoBot.Requests;
using System.Threading;

/// <summary>
/// Handles the Interaction with the "View evidence" button in a team's board channel.
/// </summary>
internal class ViewEvidenceButtonHandler : InteractionHandler<ViewEvidenceButtonRequest>
{
    private const string InitialResponseMessagePrefix = "{0} Select a tile to view its evidence.";
    private const string NoTilesFoundError = "You have not submitted evidence for any tiles.";

    private readonly string tileSelectCustomId = Guid.NewGuid().ToString();

    private Message response = null!;
    private Tile? selectedTile = null;
    private SelectComponent tileSelect = null!;
    private DiscordButton closeButton = null!;
    private User User = null!;

    protected override async Task Process(ViewEvidenceButtonRequest request, CancellationToken cancellationToken)
    {
        await base.Process(request, cancellationToken);

        User = request.User;

        closeButton = ButtonFactory.CreateClose(new(this), request.DiscordUser);
        CreateTileSelect();
        response = GetResponseMessage();
    }

    private void CreateTileSelect()
    {
        tileSelect = new SelectComponent("Select a tile", TileSelectItemSelected, getPageNameCallback: GetPageName);
        tileSelect.Build();
    }

    private List<SelectComponentOption> GetTileSelectOptions()
    {
        List<SelectComponentOption> tileSelectOptions = new();
        foreach (Tile tile in User.Team.Tiles)
        {
            TryAddSelectOption(tile, tileSelectOptions);
        }

        return tileSelectOptions;
    }

    private void TryAddSelectOption(Tile tile, List<SelectComponentOption> tileSelectOptions)
    {
        Evidence? evidence = tile.GetEvidence(DataWorker, User!.DiscordUserId);
        if (evidence is null) { return; }

        DiscordEmoji? discordEmoji = BingoBotCommon.GetEvidenceStatusEmoji(evidence);
        DiscordComponentEmoji? emoji = discordEmoji is null ? null : new DiscordComponentEmoji(discordEmoji);
        tileSelectOptions.Add(new SelectComponentItem(tile.Task.Name, tile, null, selectedTile == tile, emoji));
    }

    private Message GetResponseMessage() =>
        new Message()
            .WithContent(InitialResponseMessagePrefix.FormatConst(Interaction.User.Mention))
            .AddComponents(tileSelect)
            .AddComponents(closeButton);

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

    private string GetPageName(IEnumerable<SelectComponentOption> options) =>
        $"{options.ElementAt(0).label} - {options.ElementAt(options.Count() - 1).label}";

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