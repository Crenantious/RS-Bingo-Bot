// <copyright file="SubmitEvidenceMessageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;

public record SubmitEvidenceMessageRequest(SubmitEvidenceButtonDTO DTO, DiscordUser User, MessageFile EvidenceFile,
    InteractionMessage ResponseOverride) : IMessageCreatedRequest, IInteractionResponseOverride;