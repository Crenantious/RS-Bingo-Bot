// <copyright file="CreateTeamEvidenceChannelRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using MediatR;
using RSBingoBot.Discord;

internal record CreateTeamEvidenceChannelRequest(DiscordTeam DiscordTeam) : IRequest<Result>;