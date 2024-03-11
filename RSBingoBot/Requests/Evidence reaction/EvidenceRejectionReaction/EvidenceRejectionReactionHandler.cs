// <copyright file="EvidenceRejectionReactionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.DAL;
using RSBingo_Framework.Records;
using static RSBingo_Framework.Records.EvidenceRecord;

internal class EvidenceRejectionReactionHandler : EvidenceReactionHandler<EvidenceRejectionReactionRequest>
{
    protected override async Task Process(EvidenceRejectionReactionRequest request, CancellationToken cancellationToken)
    {
        await base.Process(request, cancellationToken);

        if (Evidence is null || Evidence.IsRejected())
        {
            // This isn't done in the validator to avoid searching the db multiple times.
            // We don't add an error here as we're just ignoring the case.
            return;
        }

        await MoveEvidenceMessage(Evidence, DataFactory.RejectedEvidenceChannel, EvidenceStatus.Rejected);
        UpdateScore();
        AddSuccess(new EvidenceRejectionReactionSuccess());
    }
}