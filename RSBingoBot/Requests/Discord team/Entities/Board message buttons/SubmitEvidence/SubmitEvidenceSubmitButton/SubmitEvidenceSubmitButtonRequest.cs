// <copyright file="SubmitEvidenceSubmitButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using RSBingo_Framework.Records;
using RSBingoBot.Discord;

internal record SubmitEvidenceSubmitButtonRequest(DiscordTeam DiscordTeam, SubmitEvidenceButtonDTO DTO,
    EvidenceRecord.EvidenceType EvidenceType, SubmitEvidenceTileSelect TileSelect) : IButtonRequest;