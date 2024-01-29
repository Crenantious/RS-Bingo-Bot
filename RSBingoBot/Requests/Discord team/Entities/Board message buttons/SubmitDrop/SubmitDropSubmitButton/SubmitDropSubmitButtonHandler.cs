// <copyright file="SubmitDropSubmitButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Extensions;
using FluentResults;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

internal class SubmitDropSubmitButtonHandler : ButtonHandler<SubmitDropSubmitButtonRequest>
{
    private const string PendingReviewMessagePrefix = "{0} has submitted {1} evidence for {2}{3}{4}";

    private readonly IDiscordMessageServices messageServices;
    private readonly IDatabaseServices databaseServices;

    public SubmitDropSubmitButtonHandler(IDiscordMessageServices messageServices, IDatabaseServices databaseServices)
    {
        this.messageServices = messageServices;
        this.databaseServices = databaseServices;
    }

    protected override async Task Process(SubmitDropSubmitButtonRequest request, CancellationToken cancellationToken)
    {
        User user = GetUser()!;
        // TODO: JR - check if this will work with deleted/edited tiles or if the tiles id's need to be transfered
        // so the dw can try to retrieve them.
        IEnumerable<Tile> tiles = request.DTO.Tiles;

        foreach (Tile tile in tiles)
        {
            // TODO: JR - update all evidence concurrently and only await them all as a group.
            await UpdateEvidence(request, user, tile);
        }

        // TODO: JR - update the board.

        Result result = await databaseServices.Update(DataWorker);
        if (result.IsFailed)
        {
            AddErrors(result.Errors);
        }
    }

    private async Task UpdateEvidence(SubmitDropSubmitButtonRequest request, User user, Tile tile)
    {
        if (tile.IsCompleteAsBool())
        {
            // It's possible the tile was marked as complete after the select component was create.
            AddErrorResponse(new SubmitDropSubmitButtonTileAlreadyCompleteError(tile));
            return;
        }

        var pendingReviewMessage = new Message()
            .WithContent(GetPendingReviewMessagePrefix(request, tile));

        // TODO: JR - move the singleton channels to a DTO to use as a singleton with DI.
        Result result = await messageServices.Send(pendingReviewMessage, DataFactory.PendingReviewEvidenceChannel);

        if (result.IsFailed)
        {
            AddErrorResponse(new SubmitDropSubmitButtonEvidenceSubmissionError(tile));
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

        AddSuccessResponse(new SubmitDropSubmitButtonSuccess(tile));
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
        new(PendingReviewMessagePrefix.FormatConst(request.GetDiscordInteraction().User.Mention, request.EvidenceType,
            tile.Task.Name, Environment.NewLine, request.DTO.EvidenceUrl!));
}