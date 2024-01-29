// <copyright file="SubmitDropSubmitButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using RSBingo_Framework.Records;

public record SubmitDropSubmitButtonRequest(SubmitDropButtonDTO DTO, EvidenceRecord.EvidenceType EvidenceType) : IButtonRequest;