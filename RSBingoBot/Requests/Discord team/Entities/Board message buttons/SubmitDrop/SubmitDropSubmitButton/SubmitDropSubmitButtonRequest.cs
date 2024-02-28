// <copyright file="SubmitDropSubmitButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingoBot.Discord;

public record SubmitDropSubmitButtonRequest(IDataWorker DataWorker, User User, DiscordTeam DiscordTeam, SubmitDropButtonDTO DTO,
    EvidenceRecord.EvidenceType EvidenceType, SubmitEvidenceTileSelect TileSelect) : IButtonRequest;