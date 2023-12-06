﻿// <copyright file="SubmitDropSubmitButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.RequestHandlers;
using FluentResults;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

internal class SubmitDropSubmitButtonHandler : ButtonHandler<SubmitDropSubmitButtonRequest>
{
    private const string PendingReviewMessagePrefix = "{0} has submitted {1} evidence for {2}{3}{4}";

    private readonly IDiscordMessageServices messageServices;

    public SubmitDropSubmitButtonHandler(IDiscordMessageServices messageServices)
    {
        this.messageServices = messageServices;
    }

    protected override async Task Process(SubmitDropSubmitButtonRequest request, CancellationToken cancellationToken)
    {
        User user = GetUser()!;
        IEnumerable<Tile> tiles = request.DTO.Tiles;

        foreach (Tile tile in tiles)
        {
            // TODO: JR - update all evidence concurrently and only await them all as a group.
            await UpdateEvidence(request, user, tile);
        }

        // TODO: JR - update the board.
        // TODO: JR - make altering a Team a request so a semaphore can be used which will avoid concurrency issues and
        // can return any db errors.
        DataWorker.SaveChanges();
    }

    private async Task UpdateEvidence(SubmitDropSubmitButtonRequest request, User user, Tile tile)
    {
        if (tile.IsCompleteAsBool())
        {
            // It's possible the tile was marked as complete after the select component was create.
            AddError(new SubmitDropSubmitButtonTileAlreadyCompleteError(tile));
            return;
        }

        var pendingReviewMessage = new Message()
            .WithContent(GetPendingReviewMessagePrefix(request, tile));

        // TODO: JR - move the singleton channels to a DTO to use as a singleton with DI.
        Result result = await messageServices.Send(pendingReviewMessage, DataFactory.PendingReviewEvidenceChannel);

        if (result.IsFailed)
        {
            AddError(new SubmitDropSubmitButtonEvidenceSubmissionError(tile));
            return;
        }

        Evidence? evidence = EvidenceRecord.GetByTileUserAndType(DataWorker, tile, user, request.EvidenceType);

        if (evidence is null)
        {
            evidence = DataWorker.Evidence.Create();
        }
        else
        {
            await DeleteExistingPendingReviewMessage(evidence);
        }

        evidence.User = user;
        evidence.Tile = tile;
        evidence.Url = request.DTO.EvidenceUrl!;
        evidence.EvidenceType = EvidenceRecord.EvidenceTypeLookup.Get(request.EvidenceType);
        evidence.Status = EvidenceRecord.EvidenceStatusLookup.Get(EvidenceRecord.EvidenceStatus.PendingReview);
        evidence.DiscordMessageId = pendingReviewMessage.DiscordMessage.Id;

        AddSuccess(new SubmitDropSubmitButtonSuccess(tile));
    }

    private async Task DeleteExistingPendingReviewMessage(Evidence evidence)
    {
        Result<Message> message = await messageServices.Get(evidence.DiscordMessageId, DataFactory.PendingReviewEvidenceChannel);
        if (message.IsSuccess)
        {
            await messageServices.Delete(message.Value.DiscordMessage);
        }
    }

    private static string GetPendingReviewMessagePrefix(SubmitDropSubmitButtonRequest request, Tile tile) =>
        new(PendingReviewMessagePrefix.FormatConst(request.InteractionArgs.User.Mention, request.EvidenceType,
            tile.Task.Name, Environment.NewLine, request.DTO.EvidenceUrl!));
}