﻿// <copyright file="ViewEvidenceButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Factories;
using DiscordLibrary.RequestHandlers;
using DSharpPlus.Entities;
using RSBingo_Framework.Models;
using RSBingoBot.Requests;

/// <summary>
/// Handles the Interaction with the "View evidence" button in a team's board channel.
/// </summary>
internal class ViewEvidenceButtonHandler : ButtonHandler<ViewEvidenceButtonRequest>
{
    private readonly SelectComponentFactory selectComponentFactory;
    private readonly ButtonFactory buttonFactory;

    private InteractionMessage? response;

    public ViewEvidenceButtonHandler(SelectComponentFactory selectComponentFactory, ButtonFactory buttonFactory)
    {
        this.selectComponentFactory = selectComponentFactory;
        this.buttonFactory = buttonFactory;
    }

    protected override async Task Process(ViewEvidenceButtonRequest request, CancellationToken cancellationToken)
    {
        await base.Process(request, cancellationToken);

        var selectComponent = CreateSelectComponent();
        var closeButton = buttonFactory.Create(ButtonFactory.CloseButton, new ConclueInteractionButtonRequest(this));
        response = AddSuccess(new ViewEvidenceSuccess(request.InteractionArgs.Interaction, DiscordUser, selectComponent, closeButton));
    }

    public override async Task Conclude()
    {
        if (response is not null)
        {
            await response.Delete();
        }
        await base.Conclude();
    }

    private SelectComponent CreateSelectComponent() =>
        selectComponentFactory.Create(
            new SelectComponentInfo("Select a tile", GetSelectOptions()),
            new ViewEvidenceSelectRequest());

    private IEnumerable<SelectComponentOption> GetSelectOptions() =>
        User.Evidence.Select(e => CreateSelectOption(e));

    private SelectComponentItem CreateSelectOption(Evidence evidence) =>
        new(evidence.Tile.Task.Name, evidence, emoji: GetSelectOptionEmoji(evidence));

    private static DiscordComponentEmoji? GetSelectOptionEmoji(Evidence evidence)
    {
        DiscordEmoji? discordEmoji = BingoBotCommon.GetEvidenceStatusEmoji(evidence);
        return discordEmoji is null ? null : new DiscordComponentEmoji(discordEmoji);
    }
}