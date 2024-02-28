// <copyright file="SubmitDropMessageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;

public record SubmitDropMessageRequest(SubmitDropButtonDTO DTO, InteractionMessage ResponseOverride) :
    IMessageCreatedRequest, IInteractionResponseOverride;