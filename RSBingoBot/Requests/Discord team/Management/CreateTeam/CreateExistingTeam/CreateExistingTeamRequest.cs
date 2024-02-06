// <copyright file="CreateExistingTeamRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using MediatR;
using RSBingo_Framework.Models;
using RSBingoBot.Discord;

internal record CreateExistingTeamRequest(Team Team) : IRequest<Result<DiscordTeam>>;