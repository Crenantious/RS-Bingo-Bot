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

// TODO: JR - don't allow tiles to be changed while someone is submitting evidence and vice versa.
internal class SubmitDropButtonHandler : ButtonHandler<SubmitDropButtonRequest>
{
    private const string ResponsePrefix =
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
        evidenceType = request.EvidenceType;

        var response = new InteractionMessage(InteractionArgs.Interaction)
             .WithContent(GetResponsePrefix())
             .AsEphemeral(true);
        SubmitDropButtonDTO dto = new(response);

        Button submit = buttonFactory.Create(new(ButtonStyle.Primary, "Submit"), new SubmitDropSubmitButtonRequest(dto, evidenceType));
        Button cancel = buttonFactory.Create(new(ButtonStyle.Primary, "Cancel"), new ConclueInteractionButtonRequest(this));

        response.AddComponents(CreateSelectComponent(request.maxSelectOptions, dto));
        response.AddComponents(submit, cancel);
        ResponseMessages.Add(response);

        messageServices.RegisterMessageCreatedHandler(new SubmitDropMessageRequest(dto), new(InteractionArgs.Channel, InteractionArgs.User, 1));
    }

    private string GetResponsePrefix() =>
        ResponsePrefix.FormatConst(Environment.NewLine);

    private SelectComponent CreateSelectComponent(int maxOptions, SubmitDropButtonDTO dto) =>
        selectFactory.Create(
            new SelectComponentInfo("Select a tile", GetSelectOptions(), MaxOptions: maxOptions),
            new SubmitDropSelectRequest(dto));

    private IEnumerable<SelectComponentOption> GetSelectOptions() =>
        user.Team.Tiles
            .Where(t => t.IsCompleteAsBool() is false)
            .Select(t => CreateSelectOption(t));

    private SelectComponentItem CreateSelectOption(Tile tile) =>
        new(tile.Task.Name, tile, emoji: GetSelectOptionEmoji(tile));

    private DiscordComponentEmoji? GetSelectOptionEmoji(Tile tile)
    {
        Evidence? evidence = EvidenceRecord.GetByTileUserAndType(DataWorker, tile, user, evidenceType);
        if (evidence == default)
        {
            return default;
        }

        // TODO: JR - get the emoji for the user's evidence if they are submitting evidence,
        // but get it for any drop evidence if they are submitting a drop.
        DiscordEmoji? discordEmoji = BingoBotCommon.GetEvidenceStatusEmoji(evidence);
        return discordEmoji is null ? null : new DiscordComponentEmoji(discordEmoji);
    }
}