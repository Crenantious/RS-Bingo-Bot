// <copyright file="SubmitEvidenceSelectRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;

public record SubmitEvidenceSelectRequest(SubmitEvidenceButtonDTO DTO) : ISelectComponentRequest;