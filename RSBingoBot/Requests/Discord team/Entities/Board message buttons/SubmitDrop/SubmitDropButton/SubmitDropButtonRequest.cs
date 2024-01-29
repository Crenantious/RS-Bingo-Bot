// <copyright file="SubmitDropButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using RSBingo_Framework.Records;
using RSBingoBot.Discord;

public record SubmitDropButtonRequest(DiscordTeam DiscordTeam, EvidenceRecord.EvidenceType EvidenceType, int maxSelectOptions) : IButtonRequest;