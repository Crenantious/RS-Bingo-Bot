// <copyright file="RenameTeamRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using MediatR;
using RSBingoBot.Discord;

public record RenameTeamRequest(DiscordTeam DiscordTeam, string NewName) : IRequest<Result>;