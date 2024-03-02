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

    private readonly ButtonFactory buttonFactory;
    private readonly SelectComponentFactory selectFactory;
    private readonly IEvidenceVerificationEmojis evidenceVerificationEmojis;
    private User user = null!;
    private EvidenceRecord.EvidenceType evidenceType;
    private IDataWorker dataWorker = null!;

    protected override bool SendKeepAliveMessageIsEphemeral => false;
    public SubmitEvidenceButtonHandler(ButtonFactory buttonFactory, SelectComponentFactory selectFactory,
        IEvidenceVerificationEmojis evidenceVerificationEmojis)
    {
        this.buttonFactory = buttonFactory;
        this.selectFactory = selectFactory;
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

        var response = new InteractionMessage(Interaction)
             .WithContent(GetResponsePrefix(Interaction.User))
             .AddFile(evidenceFile)
             .AsEphemeral(true);

        SubmitEvidenceButtonDTO dto = new(response);

        SubmitEvidenceTileSelect tileSelect = new(dataWorker, dto, user, request.EvidenceType, evidenceVerificationEmojis);
        Button submit = buttonFactory.Create(new(ButtonStyle.Primary, "Submit"),
            () => new SubmitEvidenceSubmitButtonRequest(dataWorker, user, request.DiscordTeam, dto, evidenceType, tileSelect));
        Button cancel = buttonFactory.CreateConcludeInteraction(() => new(InteractionTracker, new List<Message>() { response }, Interaction.User));

        response.AddComponents(tileSelect.SelectComponent);
        response.AddComponents(submit, cancel);

        messageServices.RegisterMessageCreatedHandler(
            () => new SubmitEvidenceMessageRequest(dto, Interaction.User, evidenceFile, new InteractionMessage(Interaction).AsEphemeral(true)),
            new(Interaction.Channel, Interaction.User, 1));

        await interactionMessageServices.Send(response);
    }

    private string GetResponsePrefix(DiscordUser user) =>
        ResponsePrefix.FormatConst(user.Mention, Environment.NewLine);
}