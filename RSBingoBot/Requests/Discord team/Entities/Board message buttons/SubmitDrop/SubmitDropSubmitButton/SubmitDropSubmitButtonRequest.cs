// <copyright file="SubmitDropSubmitButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using RSBingo_Framework.Records;
using RSBingoBot.Discord;

public record SubmitDropSubmitButtonRequest(DiscordTeam DiscordTeam, SubmitDropButtonDTO DTO, EvidenceRecord.EvidenceType EvidenceType) :
    IButtonRequest;