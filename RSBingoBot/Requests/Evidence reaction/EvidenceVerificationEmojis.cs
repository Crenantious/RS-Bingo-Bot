// <copyright file="EvidenceVerificationEmojis.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus;
using DSharpPlus.Entities;
using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.EvidenceRecord;

internal class EvidenceVerificationEmojis : IEvidenceVerificationEmojis
{
    public DiscordEmoji PendingReview { get; }
    public DiscordEmoji Verified { get; }
    public DiscordEmoji Rejected { get; }

    public EvidenceVerificationEmojis(DiscordClient client)
    {
        PendingReview = DiscordEmoji.FromUnicode(client, "⌛");
        Verified = DiscordEmoji.FromUnicode(client, "✅");
        Rejected = DiscordEmoji.FromUnicode(client, "❌");
    }

    public DiscordEmoji? GetStatusEmoji(Evidence evidence) =>
        evidence.Status switch
        {
            (sbyte)EvidenceStatus.PendingReview => PendingReview,
            (sbyte)EvidenceStatus.Accepted => Verified,
            (sbyte)EvidenceStatus.Rejected => Rejected,
            _ => null
        };
}
