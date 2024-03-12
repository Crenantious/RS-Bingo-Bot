// <copyright file="SubmitEvidenceButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using RSBingo_Framework.Records;
using RSBingoBot.Discord;

public record SubmitEvidenceButtonRequest(DiscordTeam DiscordTeam, EvidenceRecord.EvidenceType EvidenceType,
    int maxSelectOptions) : IButtonRequest;