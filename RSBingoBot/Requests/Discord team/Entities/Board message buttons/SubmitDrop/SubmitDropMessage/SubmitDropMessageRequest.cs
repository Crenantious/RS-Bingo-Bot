// <copyright file="SubmitDropMessageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;

public record SubmitDropMessageRequest(SubmitDropButtonDTO DTO) : IMessageCreatedRequest;