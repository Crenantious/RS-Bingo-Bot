// <copyright file="EvidenceVerificationReactionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.DAL;
using RSBingo_Framework.Records;
using static RSBingo_Framework.Records.EvidenceRecord;

internal class EvidenceVerificationReactionHandler : EvidenceReactionHandler<EvidenceVerificationReactionRequest>
{
    protected override async Task Process(EvidenceVerificationReactionRequest request, CancellationToken cancellationToken)
    {
        await base.Process(request, cancellationToken);

        if (Evidence is null || Evidence.IsVerified())
        {
            // This isn't done in the validator to avoid searching the db multiple times.
            // We don't add an error here as we're just ignoring the case.
            return;
        }

        await MoveEvidenceMessage(Evidence, DataFactory.VerifiedEvidenceChannel, EvidenceStatus.Accepted);
        AddSuccess(new EvidenceVerificationReactionSuccess());
    }
}