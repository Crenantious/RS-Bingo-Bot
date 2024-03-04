// <copyright file="SubmitEvidenceSubmitButtonHandler.cs" company="PlaceholderCompany">
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

internal class SubmitEvidenceSubmitButtonHandler : ButtonHandler<SubmitEvidenceSubmitButtonRequest>
{
    private const string PendingReviewMessagePrefix = "{0} Has submitted {1} evidence for {2}.";

    private readonly MessageFactory messageFactory;

    private IDiscordMessageServices messageServices = null!;

    public SubmitEvidenceSubmitButtonHandler(MessageFactory messageFactory)
    {
        this.messageFactory = messageFactory;
    }

    protected override async Task Process(SubmitEvidenceSubmitButtonRequest request, CancellationToken cancellationToken)
    {
        messageServices = GetRequestService<IDiscordMessageServices>();
        var databaseServices = GetRequestService<IDatabaseServices>();

        IEnumerable<Tile> tiles = request.DTO.Tiles;

        foreach (Tile tile in tiles)
        {
            // TODO: JR - update all evidence concurrently and only await them all as a group.
            await UpdateEvidence(request, tile);
        }

        // TODO: JR - update the board.

        Result result = await databaseServices.SaveChanges(request.DataWorker);
        if (result.IsFailed)
        {
            AddErrors(result.Errors);
        }

        request.TileSelect.Update(tiles);
        await messageServices.Update(request.DTO.Message);
    }

    private async Task UpdateEvidence(SubmitEvidenceSubmitButtonRequest request, Tile tile)
    {
        if (tile.IsCompleteAsBool())
        {
            // It's possible the tile was marked as complete after the select component was created.
            AddError(new SubmitEvidenceSubmitButtonTileAlreadyCompleteError(tile));
            return;
        }

        var pendingReviewMessage = await SendPendingReviewMessage(request, tile);

        if (pendingReviewMessage.IsFailed)
        {
            AddError(new SubmitEvidenceSubmitButtonEvidenceSubmissionError(tile));
            return;
        }

        Evidence? evidence = EvidenceRecord.GetByTileUserAndType(request.DataWorker, tile, request.User, request.EvidenceType);

        if (evidence is null)
        {
            evidence = request.DataWorker.Evidence.Create();
        }
        else
        {
            await DeleteExistingPendingReviewMessage(evidence);
        }

        evidence.User = request.User;
        evidence.Tile = tile;
        evidence.Url = request.DTO.EvidenceUrl!;
        evidence.EvidenceType = EvidenceRecord.EvidenceTypeLookup.Get(request.EvidenceType);
        evidence.Status = EvidenceRecord.EvidenceStatusLookup.Get(EvidenceRecord.EvidenceStatus.PendingReview);
        evidence.DiscordMessageId = pendingReviewMessage.Value.DiscordMessage.Id;

        AddSuccess(new SubmitEvidenceSubmitButtonSuccess(tile));
    }

    private async Task<Result<Message>> SendPendingReviewMessage(SubmitEvidenceSubmitButtonRequest request, Tile tile)
    {
        var file = request.DTO.Message.Files.ElementAt(0);
        var pendingReviewMessage = messageFactory.Create(DataFactory.PendingReviewEvidenceChannel)
            .WithContent(GetPendingReviewMessageContent(request, tile))
            .AddFile(file);

        // TODO: JR - move the singleton channels to a DTO to use as a singleton with DI.
        Result result = await messageServices.Send(pendingReviewMessage);
        return new Result<Message>()
            .WithValue(pendingReviewMessage)
            .WithErrors(result.Errors);
    }

    private async Task DeleteExistingPendingReviewMessage(Evidence evidence)
    {
        Result<Message> message = await messageServices.Get(evidence.DiscordMessageId, DataFactory.PendingReviewEvidenceChannel);
        if (message.IsSuccess)
        {
            await messageServices.Delete(message.Value.DiscordMessage);
        }
    }

    private static string GetPendingReviewMessageContent(SubmitEvidenceSubmitButtonRequest request, Tile tile) =>
        PendingReviewMessagePrefix.FormatConst(
            request.GetDiscordInteraction().User.Mention,
            request.EvidenceType,
            tile.Task.Name);
}