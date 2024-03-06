// <copyright file="EvidenceRejectionReactionRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;

internal record EvidenceRejectionReactionRequest(DiscordEmoji VerificationEmoji) : EvidenceReactionRequest;