// <copyright file="EvidenceVerificationReactionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.EvidenceRecord;

internal class EvidenceVerificationReactionHandler : EvidenceReactionHandler<EvidenceVerificationReactionRequest>
{
    protected override DiscordChannel ChannelToMoveEvidenceTo { get; set; } = DataFactory.VerifiedEvidenceChannel;
    protected override EvidenceStatus NewEvidenceStatus { get; set; } = EvidenceStatus.Accepted;

    protected override bool ValidateEvidence(Evidence evidence) =>
        evidence.HasStatus(EvidenceStatus.PendingReview);
}