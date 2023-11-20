// <copyright file="ViewEvidenceButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordExtensions;
using DiscordLibrary.Factories;
using DiscordLibrary.RequestHandlers;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingoBot.Requests;
using System.Threading;

/// <summary>
/// Handles the Interaction with the "View evidence" button in a team's board channel.
/// </summary>
internal class ViewEvidenceButtonHandler : ButtonHandler<ViewEvidenceButtonRequest>
{
    private const string messageText = "{0} Select a tile to view its evidence.";
    private const string NoTilesFoundError = "You have not submitted evidence for any tiles.";

    private readonly SelectComponentFactory selectComponentFactory;

    private Message response = null!;
    private Tile? evidenceTile = null;
    private User User = null!;

    public ViewEvidenceButtonHandler(SelectComponentFactory selectComponentFactory)
    {
        this.selectComponentFactory = selectComponentFactory;
    }

    protected override async Task Process(ViewEvidenceButtonRequest request, CancellationToken cancellationToken)
    {
        await base.Process(request, cancellationToken);

        User = request.InteractionArgs.Interaction.User.GetDBUser(DataWorker)!;

        CreateEvidenceSelection(request.Interaction.User);
        closeButton = ButtonFactory.CreateClose(new CloseButtonRequest(this), request.Interaction.User);
        response = GetResponseMessage();
    }

    private void CreateEvidenceSelection(DiscordUser user)
    {
        selectComponentFactory.Create(
            new SelectComponentInfo("Select a tile", GetEvidenceSelectionOptions()),
            new ViewEvidenceSelectionRequest(),
            new (User: user));
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

    private async Task CancelButtonInteraction(DiscordClient client, ComponentInteractionCreateEventArgs args) =>
        await Conclude();
}