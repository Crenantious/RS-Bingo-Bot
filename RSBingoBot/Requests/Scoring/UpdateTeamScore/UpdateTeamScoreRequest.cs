// <copyright file="UpdateTeamScoreRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using MediatR;
using RSBingo_Framework.Models;
using RSBingoBot.Discord;

internal record UpdateTeamScoreRequest(DiscordTeam DiscordTeam, Team Team) : IRequest<Result>;