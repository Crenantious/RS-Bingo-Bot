// <copyright file="ViewEvidenceButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordExtensions;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Factories;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingoBot.Requests;

internal class ViewEvidenceButtonHandler : ButtonHandler<ViewEvidenceButtonRequest>
{
    private const string ResponseContent = "{0} Select a tile to view its evidence.";

    private readonly SelectComponentFactory selectComponentFactory;
    private readonly ButtonFactory buttonFactory;
    private readonly InteractionMessageFactory interactionMessageFactory;
    private readonly IEvidenceVerificationEmojis evidenceVerificationEmojis;

    public ViewEvidenceButtonHandler(SelectComponentFactory selectComponentFactory, ButtonFactory buttonFactory,
        InteractionMessageFactory interactionMessageFactory, IEvidenceVerificationEmojis evidenceVerificationEmojis)
    {
        this.selectComponentFactory = selectComponentFactory;
        this.buttonFactory = buttonFactory;
        this.interactionMessageFactory = interactionMessageFactory;
        this.evidenceVerificationEmojis = evidenceVerificationEmojis;
    }

    protected override async Task Process(ViewEvidenceButtonRequest request, CancellationToken cancellationToken)
    {
        IDataWorker dataWorker = DataFactory.CreateDataWorker();

        DiscordUser discordUser = Interaction.User;
        User user = discordUser.GetDBUser(dataWorker)!;

        var selectComponent = CreateSelectComponent(user);
        var closeButton = buttonFactory.CreateConcludeInteraction(() => new ConcludeInteractionButtonRequest(InteractionTracker));

        var response = interactionMessageFactory.Create(Interaction)
                .WithContent(ResponseContent.FormatConst(discordUser.Mention))
                .AddComponents(selectComponent)
                .AddComponents(closeButton);

        var messageServices = GetRequestService<IDiscordInteractionMessagingServices>();
        await messageServices.Send(response);
    }

    private SelectComponent CreateSelectComponent(User user) =>
        selectComponentFactory.Create(
            new(new SelectComponentPage("Select a tile", GetSelectOptions(user))),
            () => new ViewEvidenceSelectRequest());

    private IEnumerable<SelectComponentOption> GetSelectOptions(User user) =>
        user.Evidence.Select(e => CreateSelectOption(e));

    private SelectComponentItem CreateSelectOption(Evidence evidence) =>
        new(evidence.Tile.Task.Name, evidence, emoji: GetSelectOptionEmoji(evidence));

    private DiscordComponentEmoji? GetSelectOptionEmoji(Evidence evidence)
    {
        DiscordEmoji? discordEmoji = evidenceVerificationEmojis.GetStatusEmoji(evidence);
        return discordEmoji is null ? null : new DiscordComponentEmoji(discordEmoji);
    }
}