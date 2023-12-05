// <copyright file="SubmitDropButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Factories;
using DiscordLibrary.RequestHandlers;
using DSharpPlus;
using DSharpPlus.Entities;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

internal class SubmitDropButtonHandler : ButtonHandler<SubmitDropButtonRequest>
{
    private const string ResponseContent =
       "{0} Add evidence by posting a message with a single image, posting another will override the previous." +
       "{1}Submitting the evidence will override any previous.";

    private readonly ButtonFactory buttonFactory;
    private readonly SelectComponentFactory selectFactory;

    private User user;
    private EvidenceRecord.EvidenceType evidenceType;

    public SubmitDropButtonHandler(ButtonFactory buttonFactory, SelectComponentFactory selectFactory)
    {
        this.buttonFactory = buttonFactory;
        this.selectFactory = selectFactory;
    }

    protected override async Task Process(SubmitDropButtonRequest request, CancellationToken cancellationToken)
    {
        user = GetUser()!;
        evidenceType = General.HasCompetitionStarted ?
                       EvidenceRecord.EvidenceType.Drop :
                       EvidenceRecord.EvidenceType.TileVerification;

        SelectComponent select = CreateSelectComponent();
        Button close = buttonFactory.Create(new(ButtonStyle.Primary, "Cancel"), new ConclueInteractionButtonRequest(this));
        Button submit = buttonFactory.Create(new(ButtonStyle.Primary, "Submit"),
            new SubmitDropSubmitButtonRequest(() => GetSelectedTile(select), GetAttachment, evidenceType));

        ResponseMessages.Add(
            new InteractionMessage(InteractionArgs.Interaction)
                .WithContent(GetResponseContent())
                .AddComponents(select)
                .AddComponents(submit, close));
    }

    private SelectComponent CreateSelectComponent() =>
        selectFactory.Create(
            new SelectComponentInfo("Select a tile", GetSelectOptions()),
            new ViewEvidenceSelectRequest());

    private IEnumerable<SelectComponentOption> GetSelectOptions() =>
        user.Team.Tiles.Select(t => CreateSelectOption(t));

    private SelectComponentItem CreateSelectOption(Tile tile) =>
        new(tile.Task.Name, tile, emoji: GetSelectOptionEmoji(tile));

    private DiscordComponentEmoji? GetSelectOptionEmoji(Tile tile)
    {
        Evidence? evidence = EvidenceRecord.GetByTileUserAndType(DataWorker, tile, user, evidenceType);
        if (evidence == default)
        {
            return default;
        }

        DiscordEmoji? discordEmoji = BingoBotCommon.GetEvidenceStatusEmoji(evidence);
        return discordEmoji is null ? null : new DiscordComponentEmoji(discordEmoji);
    }

    private Tile? GetSelectedTile(SelectComponent select) =>
        select.SelectedItems.Any() ? (Tile)select.SelectedItems.ElementAt(0).Value! : null;

    private DiscordAttachment? GetAttachment()
    {
        throw new NotImplementedException();
    }

    private string GetResponseContent() =>
        ResponseContent.FormatConst(InteractionArgs.Interaction.User.Mention, Environment.NewLine);
}