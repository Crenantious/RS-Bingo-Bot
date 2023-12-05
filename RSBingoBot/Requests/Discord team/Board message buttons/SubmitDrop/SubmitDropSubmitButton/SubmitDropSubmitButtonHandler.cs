// <copyright file="SubmitDropSubmitButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.RequestHandlers;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

internal class SubmitDropSubmitButtonHandler : ButtonHandler<SubmitDropSubmitButtonRequest>
{
    public SubmitDropSubmitButtonHandler()
    {

    }

    protected override async Task Process(SubmitDropSubmitButtonRequest request, CancellationToken cancellationToken)
    {
        User user = GetUser()!;
        Tile tile = request.GetTile()!;

        Evidence? evidence = EvidenceRecord.GetByTileUserAndType(DataWorker, tile, user, request.EvidenceType);

        if (evidence == null)
        {
            DataWorker.Evidence.Create();
            return;
        }

        evidence.User = user;
        evidence.Tile = tile;
        evidence.Url = request.GetUrl()!;
        evidence.EvidenceType = EvidenceRecord.EvidenceTypeLookup.Get(request.EvidenceType);
        evidence.Status = EvidenceRecord.EvidenceStatusLookup.Get(EvidenceRecord.EvidenceStatus.PendingReview);
        evidence.DiscordMessageId = ; // TODO: JR - send submission message and set the id.

        AddSuccess(new SubmitDropSubmitButtonSuccess(tile));
    }
}