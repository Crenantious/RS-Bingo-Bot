// <copyright file="EvidenceVerificationReactionRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using DSharpPlus.Entities;

internal record EvidenceVerificationReactionRequest(DiscordEmoji VerificationEmoji) : IMessageReactedRequest;