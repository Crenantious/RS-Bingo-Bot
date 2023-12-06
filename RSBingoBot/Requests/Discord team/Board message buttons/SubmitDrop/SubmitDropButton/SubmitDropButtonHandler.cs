// <copyright file="SubmitDropButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Factories;
using DiscordLibrary.RequestHandlers;
using DSharpPlus;
using DSharpPlus.Entities;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

internal class SubmitDropButtonHandler : ButtonHandler<SubmitDropButtonRequest>
{
    private const string ResponseContent =
       "Add evidence by posting a message with a single image, posting another will override the previous." +
       "{1}Submitting the evidence will override any previous.";

    private readonly ButtonFactory buttonFactory;
    private readonly SelectComponentFactory selectFactory;
    private readonly IDiscordMessageServices messageServices;

    private User user = null!;
    private EvidenceRecord.EvidenceType evidenceType;

    public SubmitDropButtonHandler(ButtonFactory buttonFactory, SelectComponentFactory selectFactory, IDiscordMessageServices messageServices)
    {
        this.buttonFactory = buttonFactory;
        this.selectFactory = selectFactory;
        this.messageServices = messageServices;
    }

    protected override async Task Process(SubmitDropButtonRequest request, CancellationToken cancellationToken)
    {
        user = GetUser()!;
        evidenceType = General.HasCompetitionStarted ?
                       EvidenceRecord.EvidenceType.Drop :
                       EvidenceRecord.EvidenceType.TileVerification;

        var response = new InteractionMessage(InteractionArgs.Interaction)
             .WithContent(GetResponseContent())
             .AsEphemeral(true);
        SubmitDropButtonDTO dto = new(response);

        Button submit = buttonFactory.Create(new(ButtonStyle.Primary, "Submit"), new SubmitDropSubmitButtonRequest(dto, evidenceType));
        Button cancel = buttonFactory.Create(new(ButtonStyle.Primary, "Cancel"), new ConclueInteractionButtonRequest(this));

        response.AddComponents(CreateSelectComponent(dto));
        response.AddComponents(submit, cancel);
        ResponseMessages.Add(response);

        messageServices.RegisterMessageCreatedHandler(new SubmitDropMessageRequest(dto), new(InteractionArgs.Channel, InteractionArgs.User, 1));
    }
    private string GetResponseContent() =>
        ResponseContent.FormatConst(Environment.NewLine);

    private SelectComponent CreateSelectComponent(SubmitDropButtonDTO dto) =>
        selectFactory.Create(
            new SelectComponentInfo("Select a tile", GetSelectOptions()),
            new SubmitDropSelectRequest(dto));

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
}