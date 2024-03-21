﻿// <copyright file="SubmitEvidenceButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordExtensions;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Factories;
using DiscordLibrary.Requests;
using DSharpPlus;
using DSharpPlus.Entities;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

internal class SubmitEvidenceButtonHandler : ButtonHandler<SubmitEvidenceButtonRequest>
{
    private const string ResponsePrefix =
       "{0} Add evidence by posting a message with a single image, posting another will override the previous. " +
       "{1}Submitting the evidence will override any previous.";
    private const string NoTilesToSubmitFor = "There are no tiles you can submit evidence for.";

    private readonly ButtonFactory buttonFactory;
    private readonly SelectComponentFactory selectFactory;
    private readonly InteractionMessageFactory interactionMessageFactory;
    private readonly IEvidenceVerificationEmojis evidenceVerificationEmojis;

    private User user = null!;
    private EvidenceRecord.EvidenceType evidenceType;
    private IDataWorker dataWorker = null!;

    protected override bool SendKeepAliveMessageIsEphemeral => false;
    public SubmitEvidenceButtonHandler(ButtonFactory buttonFactory, SelectComponentFactory selectFactory,
        InteractionMessageFactory interactionMessageFactory, IEvidenceVerificationEmojis evidenceVerificationEmojis)
    {
        this.buttonFactory = buttonFactory;
        this.selectFactory = selectFactory;
        this.interactionMessageFactory = interactionMessageFactory;
        this.evidenceVerificationEmojis = evidenceVerificationEmojis;
    }

    protected override async Task Process(SubmitEvidenceButtonRequest request, CancellationToken cancellationToken)
    {
        var messageServices = GetRequestService<IDiscordMessageServices>();
        var interactionMessageServices = GetRequestService<IDiscordInteractionMessagingServices>();

        dataWorker = DataFactory.CreateDataWorker();

        user = Interaction.User.GetDBUser(dataWorker)!;
        evidenceType = request.EvidenceType;

        MessageFile evidenceFile = new("Evidence");

        var response = interactionMessageFactory.Create(Interaction)
             .AddFile(evidenceFile);

        SubmitEvidenceButtonDTO dto = new(response);

        SubmitEvidenceTileSelect tileSelect = new(dataWorker, dto, user, request.EvidenceType, evidenceVerificationEmojis);

        var subscriptionId = RegisterForMessageCreatedEvent(messageServices, tileSelect, evidenceFile, dto);

        Button submit = CreateSubmitButton(request, dto, tileSelect);
        Button close = CreateCloseButton(response, subscriptionId);

        UpdateResponse(response, tileSelect, submit, close);

        await interactionMessageServices.Send(response);
    }

    private int? RegisterForMessageCreatedEvent(IDiscordMessageServices messageServices, SubmitEvidenceTileSelect tileSelect,
        MessageFile evidenceFile, SubmitEvidenceButtonDTO dto)
    {
        if (tileSelect.SelectComponent.Options.Any() is false)
        {
            return null;
        }

        return messageServices.RegisterMessageCreatedHandler(
            () => new SubmitEvidenceMessageRequest(dto, Interaction.User,
                  evidenceFile, interactionMessageFactory.Create(Interaction).AsEphemeral(true)),
            args => args.Channel == Interaction.Channel &&
                    args.Author == Interaction.User &&
                    args.Message.Attachments.Count() == 1);
    }

    private void UnregisterMessageCreated(IDiscordMessageServices messageServices, int id) =>
        messageServices.UnregisterMessageCreatedHandler(id);

    private Button CreateSubmitButton(SubmitEvidenceButtonRequest request, SubmitEvidenceButtonDTO dto, SubmitEvidenceTileSelect tileSelect) =>
        buttonFactory.Create(new(ButtonStyle.Primary, "Submit"),
            () => new SubmitEvidenceSubmitButtonRequest(request.DiscordTeam, dto, evidenceType, tileSelect));

    private Button CreateCloseButton(InteractionMessage response, int? subscriptionId) =>
        buttonFactory.CreateConcludeInteraction(() =>
            new SubmitEvidenceCloseButtonRequest(InteractionTracker, new List<Message>() { response }, Interaction.User, subscriptionId));

    private void UpdateResponse(InteractionMessage response, SubmitEvidenceTileSelect tileSelect, Button submit, Button close)
    {
        if (tileSelect.SelectComponent.Options.Any())
        {
            response.WithContent(GetResponsePrefix(Interaction.User))
                .AddComponents(tileSelect.SelectComponent)
                .AddComponents(submit, close);
        }
        else
        {
            response.WithContent(NoTilesToSubmitFor)
                .AddComponents(close)
                // TODO: JR - this doesn't work, probably because of the keep-alive message.
                .AsEphemeral(true);
        }
    }

    private string GetResponsePrefix(DiscordUser user) =>
        ResponsePrefix.FormatConst(user.Mention, Environment.NewLine);
}