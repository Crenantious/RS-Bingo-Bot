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
using RSBingoBot.DiscordEventHandlers;
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
internal class ViewEvidenceButtonHandler : ButtonHandler<ViewEvidenceButtonRequest>
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

        CreateEvidenceSelection(request.Interaction.User);
        closeButton = ButtonFactory.CreateClose(new CloseButtonRequest(this), request.Interaction.User);
        response = GetResponseMessage();
    }

    private void CreateEvidenceSelection(DiscordUser user)
    {
        evidenceSelection = SelectComponentFactory.Create(
            new SelectComponentInfo("Select a tile", GetEvidenceSelectionOptions()),
            new ViewEvidenceSelectionRequest(),
            new ComponentInteractionDEH.StrippedConstraints(User: user));
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