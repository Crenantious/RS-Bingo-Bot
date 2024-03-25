﻿// <copyright file="EvidenceRejectionReactionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.EvidenceRecord;

internal class EvidenceRejectionReactionHandler : EvidenceReactionHandler<EvidenceRejectionReactionRequest>
{
    protected override DiscordChannel ChannelToMoveEvidenceTo { get; set; } = DataFactory.RejectedEvidenceChannel;
    protected override EvidenceStatus NewEvidenceStatus { get; set; } = EvidenceStatus.Rejected;

    protected override bool ValidateEvidence(Evidence evidence) =>
        evidence.HasStatus(EvidenceStatus.PendingReview) ||
        evidence.HasStatus(EvidenceStatus.Accepted);
}