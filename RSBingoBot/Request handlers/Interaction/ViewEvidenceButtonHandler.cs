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
using RSBingoBot.DiscordExtensions;
using RSBingoBot.Factories;
using RSBingoBot.InteractionHandlers;
using RSBingoBot.Requests;
using System.Threading;

/// <summary>
/// Handles the Interaction with the "View evidence" button in a team's board channel.
/// </summary>
internal class ViewEvidenceButtonHandler : InteractionHandler<ViewEvidenceButtonRequest>
{
    private const string messageText = "{0} Select a tile to view its evidence.";
    private const string NoTilesFoundError = "You have not submitted evidence for any tiles.";

    private readonly string tileSelectCustomId = Guid.NewGuid().ToString();

    private Message response = null!;
    private Tile? evidenceTile = null;
    private SelectComponent evidenceSelection = null!;
    private DiscordButton closeButton = null!;
    private User User = null!;

    protected override async Task Process(ViewEvidenceButtonRequest request, CancellationToken cancellationToken)
    {
        await base.Process(request, cancellationToken);

        User = request.Interaction.User.GetDBUser(DataWorker)!;

        CreateEvidenceSelection();
        closeButton = ButtonFactory.CreateClose(new CloseButtonRequest(this), request.Interaction.User);
        response = GetResponseMessage();
    }

    private void CreateEvidenceSelection()
    {
        evidenceSelection = new SelectComponent("Select a tile", OnEvidenceSelected, getPageNameCallback: GetPageName);
        evidenceSelection.SelectOptions = GetEvidenceSelectionOptions();
        evidenceSelection.Build();
    }

    private List<SelectComponentOption> GetEvidenceSelectionOptions()
    {
        List<SelectComponentOption> options = new();
        User.Team.Tiles.ForEach(t => TryAddSelectOption(t, options));
        return options;
    }

    private void TryAddSelectOption(Tile tile, List<SelectComponentOption> tileSelectOptions)
    {
        Evidence? evidence = tile.GetEvidence(DataWorker, User.DiscordUserId);
        if (evidence is null) { return; }

        DiscordEmoji? discordEmoji = BingoBotCommon.GetEvidenceStatusEmoji(evidence);
        DiscordComponentEmoji? emoji = discordEmoji is null ? null : new DiscordComponentEmoji(discordEmoji);
        tileSelectOptions.Add(new SelectComponentItem(tile.Task.Name, tile, null, this.evidenceTile == tile, emoji));
    }

    private async Task OnEvidenceSelected(ComponentInteractionCreateEventArgs args)
    {
        evidenceTile = (Tile)evidenceSelection.SelectedItems[0].value!;
        Evidence? evidence = evidenceTile.GetEvidence(DataWorker, args.User.Id);
        if (evidence is null)
        {
            // TODO: JR - make this metod use the requests system such that adding an error will post the error message.
            // Currently, this does nothing as this method is called from an event, thus the request system cannot track it.
            // Probably make the SelectComponent have its own request and handler which can be derived from.
            AddError(new EvidenceMissingError(evidenceTile));
        }

        await PostEvidence(evidence);
    }

    private string GetPageName(IEnumerable<SelectComponentOption> options) =>
        $"{options.ElementAt(0).label} - {options.ElementAt(options.Count() - 1).label}";

    private Message GetResponseMessage() =>
      new Message()
          .WithContent(GetMessageText())
          .AddComponents(evidenceSelection)
          .AddComponents(closeButton);

    private string GetMessageText() =>
        messageText.FormatConst(Request.Interaction.User.Mention);

    private async Task PostEvidence(Evidence evidence)
    {
        await PostMessage($"Evidence submitted for tile {evidence.Tile.Task.Name} {evidence.Url}");
    }

    private async Task CancelButtonInteraction(DiscordClient client, ComponentInteractionCreateEventArgs args) =>
        await Conclude();
}