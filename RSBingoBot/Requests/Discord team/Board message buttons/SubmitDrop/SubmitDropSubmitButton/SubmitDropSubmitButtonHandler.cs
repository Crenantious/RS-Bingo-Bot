// <copyright file="SubmitDropSubmitButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.RequestHandlers;
using DSharpPlus.Entities;
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

        // TODO: JR - catch db exceptions in a base class and post a message to the user telling them the bd failed to update.
        DataWorker.SaveChanges();
    }

    private async Task UpdateEvidence(SubmitDropSubmitButtonRequest request, User user, Tile tile)
    {
        var pendingReviewMessage = new Message()
            .WithContent(GetPendingReviewMessagePrefix(request, tile));

        // TODO: JR - move the singleton channels to a DTO to use as a singleton with DI.
        Result result = await messageServices.Send(pendingReviewMessage, DataFactory.PendingReviewEvidenceChannel);

        if (result.IsFailed)
        {
            AddError(new SubmitDropSubmitButtonEvidenceReviewMessageError(tile));
            return;
        }

        Evidence? evidence = EvidenceRecord.GetByTileUserAndType(DataWorker, tile, user, request.EvidenceType);

        if (evidence is null)
        {
            evidence = DataWorker.Evidence.Create();
        }
        else
        {
            await DeleteExistingEvidenceMessage(evidence);
        }

        evidence.User = user;
        evidence.Tile = tile;
        evidence.Url = request.DTO.EvidenceUrl!;
        evidence.EvidenceType = EvidenceRecord.EvidenceTypeLookup.Get(request.EvidenceType);
        evidence.Status = EvidenceRecord.EvidenceStatusLookup.Get(EvidenceRecord.EvidenceStatus.PendingReview);
        evidence.DiscordMessageId = pendingReviewMessage.DiscordMessage.Id;

        AddSuccess(new SubmitDropSubmitButtonSuccess(tile));
    }

    private async Task DeleteExistingEvidenceMessage(Evidence evidence)
    {
        DiscordChannel channel = EvidenceRecord.EvidenceStatusLookup.Get(evidence.Status) switch
        {
            EvidenceRecord.EvidenceStatus.PendingReview => DataFactory.PendingReviewEvidenceChannel,
            EvidenceRecord.EvidenceStatus.Accepted => DataFactory.VerifiedEvidenceChannel,
            EvidenceRecord.EvidenceStatus.Rejected => DataFactory.RejectedEvidenceChannel,
            _ => throw new ArgumentOutOfRangeException()
        }; ;

        Result<Message> message = await messageServices.Get(evidence.DiscordMessageId, channel);
        if (message.IsSuccess)
        {
            await messageServices.Delete(message.Value.DiscordMessage);
        }
    }

    private static string GetPendingReviewMessagePrefix(SubmitDropSubmitButtonRequest request, Tile tile) =>
        new(PendingReviewMessagePrefix.FormatConst(request.InteractionArgs.User.Mention, request.EvidenceType,
            tile.Task.Name, Environment.NewLine, request.DTO.EvidenceUrl!));
}