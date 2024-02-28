// <copyright file="SubmitDropButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Factories;
using DiscordLibrary.Requests;
using DSharpPlus;
using DSharpPlus.Entities;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

internal class SubmitDropButtonHandler : ButtonHandler<SubmitDropButtonRequest>
{
    private const string ResponsePrefix =
       "{0}Add evidence by posting a message with a single image, posting another will override the previous. " +
       "{1}Submitting the evidence will override any previous.";

    private readonly ButtonFactory buttonFactory;
    private readonly SelectComponentFactory selectFactory;

    private User user = null!;
    private EvidenceRecord.EvidenceType evidenceType;

    protected override bool SendKeepAliveMessageIsEphemeral => false;

    public SubmitDropButtonHandler(ButtonFactory buttonFactory, SelectComponentFactory selectFactory)
    {
        this.buttonFactory = buttonFactory;
        this.selectFactory = selectFactory;
    }

    protected override async Task Process(SubmitDropButtonRequest request, CancellationToken cancellationToken)
    {
        var messageServices = GetRequestService<IDiscordMessageServices>();
        var interactionMessageServices = GetRequestService<IDiscordInteractionMessagingServices>();

        user = GetUser()!;
        evidenceType = request.EvidenceType;

        var response = new InteractionMessage(Interaction)
             .WithContent(GetResponsePrefix(Interaction.User))
             .AsEphemeral(true);

        SubmitDropButtonDTO dto = new(response);

        Button submit = buttonFactory.Create(new(ButtonStyle.Primary, "Submit"),
            () => new SubmitDropSubmitButtonRequest(request.DiscordTeam, dto, evidenceType));
        Button cancel = buttonFactory.CreateConcludeInteraction(() => new(InteractionTracker, new List<Message>() { response }, Interaction.User));

        response.AddComponents(CreateSelectComponent(request.maxSelectOptions, dto));
        response.AddComponents(submit, cancel);

        messageServices.RegisterMessageCreatedHandler(() => new SubmitDropMessageRequest(dto, new InteractionMessage(Interaction).AsEphemeral(true)),
            new(Interaction.Channel, Interaction.User, 1));

        await interactionMessageServices.Send(response);
    }

    private string GetResponsePrefix(DiscordUser user) =>
        ResponsePrefix.FormatConst(user.Mention, Environment.NewLine);

    private SelectComponent CreateSelectComponent(int maxOptions, SubmitDropButtonDTO dto) =>
        selectFactory.Create(
            new(new SelectComponentPage("Select a tile", GetSelectOptions()), MaxOptions: maxOptions),
            () => new SubmitDropSelectRequest(dto));

    private IEnumerable<SelectComponentOption> GetSelectOptions() =>
        user.Team.Tiles
            .Where(t => t.IsCompleteAsBool() is false)
            .OrderBy(t => t.BoardIndex)
            .Select(t => CreateSelectOption(t));

    private SelectComponentItem CreateSelectOption(Tile tile) =>
        new(tile.Task.Name, tile, emoji: GetSelectOptionEmoji(tile));

    private DiscordComponentEmoji? GetSelectOptionEmoji(Tile tile)
    {
        Evidence? evidence = GetEvidenceForEmoji(tile);
        if (evidence == null)
        {
            return null;
        }

        DiscordEmoji? discordEmoji = BingoBotCommon.GetEvidenceStatusEmoji(evidence);
        return discordEmoji is null ? null : new DiscordComponentEmoji(discordEmoji);
    }

    private Evidence? GetEvidenceForEmoji(Tile tile) =>
        evidenceType switch
        {
            EvidenceRecord.EvidenceType.Drop => GetFirstDropEvidence(tile),
            EvidenceRecord.EvidenceType.TileVerification => EvidenceRecord.GetByTileUserAndType(DataWorker, tile, user, evidenceType),
            _ => throw new ArgumentOutOfRangeException()
        };

    private static Evidence? GetFirstDropEvidence(Tile tile) =>
        tile.Evidence.FirstOrDefault(e =>
            EvidenceRecord.EvidenceTypeLookup.Get(e.EvidenceType) == EvidenceRecord.EvidenceType.Drop &&
            EvidenceRecord.EvidenceStatusLookup.Get(e.Status) != EvidenceRecord.EvidenceStatus.Rejected);
}