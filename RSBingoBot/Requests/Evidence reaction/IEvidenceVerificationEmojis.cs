// <copyright file="IEvidenceVerificationEmojis.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using RSBingo_Framework.Models;

internal interface IEvidenceVerificationEmojis
{
    public DiscordEmoji PendingReview { get; }
    public DiscordEmoji Verified { get; }
    public DiscordEmoji Rejected { get; }

    public DiscordEmoji? GetStatusEmoji(Evidence evidence);
}
