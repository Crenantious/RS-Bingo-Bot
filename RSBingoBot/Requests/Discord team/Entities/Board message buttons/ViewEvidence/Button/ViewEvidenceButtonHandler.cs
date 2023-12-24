// <copyright file="ViewEvidenceButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordExtensions;
using DiscordLibrary.Factories;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using RSBingo_Framework.Models;
using RSBingoBot.Requests;

internal class ViewEvidenceButtonHandler : ButtonHandler<ViewEvidenceButtonRequest>
{
    private const string ResponseContent = "{0} Select a tile to view its evidence.";

    private readonly SelectComponentFactory selectComponentFactory;
    private readonly ButtonFactory buttonFactory;

    public ViewEvidenceButtonHandler(SelectComponentFactory selectComponentFactory, ButtonFactory buttonFactory)
    {
        this.selectComponentFactory = selectComponentFactory;
        this.buttonFactory = buttonFactory;
    }

    protected override async Task Process(ViewEvidenceButtonRequest request, CancellationToken cancellationToken)
    {
        DiscordUser discordUser = InteractionArgs.Interaction.User;
        User user = discordUser.GetDBUser(DataWorker)!;

        var selectComponent = CreateSelectComponent(user);
        var closeButton = buttonFactory.Create(ButtonFactory.CloseButton, new ConcludeInteractionButtonRequest(this));

        ResponseMessages.Add(
            new InteractionMessage(InteractionArgs.Interaction)
                .WithContent(ResponseContent.FormatConst(discordUser.Mention))
                .AddComponents(selectComponent)
                .AddComponents(closeButton));
    }

    public override async Task Conclude()
    {
        DeleteResponses();
    }

    private SelectComponent CreateSelectComponent(User user) =>
        selectComponentFactory.Create(
            new SelectComponentInfo("Select a tile", GetSelectOptions(user)),
            new ViewEvidenceSelectRequest());

    private IEnumerable<SelectComponentOption> GetSelectOptions(User user) =>
        user.Evidence.Select(e => CreateSelectOption(e));

    private SelectComponentItem CreateSelectOption(Evidence evidence) =>
        new(evidence.Tile.Task.Name, evidence, emoji: GetSelectOptionEmoji(evidence));

    private static DiscordComponentEmoji? GetSelectOptionEmoji(Evidence evidence)
    {
        DiscordEmoji? discordEmoji = BingoBotCommon.GetEvidenceStatusEmoji(evidence);
        return discordEmoji is null ? null : new DiscordComponentEmoji(discordEmoji);
    }
}