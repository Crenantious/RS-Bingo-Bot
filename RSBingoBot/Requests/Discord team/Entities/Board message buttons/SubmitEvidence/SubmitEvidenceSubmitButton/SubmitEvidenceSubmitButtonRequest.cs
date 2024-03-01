// <copyright file="SubmitEvidenceSubmitButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingoBot.Discord;

internal record SubmitEvidenceSubmitButtonRequest(IDataWorker DataWorker, User User, DiscordTeam DiscordTeam, SubmitEvidenceButtonDTO DTO,
    EvidenceRecord.EvidenceType EvidenceType, SubmitEvidenceTileSelect TileSelect) : IButtonRequest;