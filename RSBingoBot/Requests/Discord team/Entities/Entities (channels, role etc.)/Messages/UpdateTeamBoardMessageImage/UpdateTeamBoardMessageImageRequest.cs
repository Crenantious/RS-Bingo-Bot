// <copyright file="UpdateTeamBoardMessageImageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using MediatR;
using RSBingo_Framework.Models;
using RSBingoBot.Discord;

internal record UpdateTeamBoardMessageImageRequest(DiscordTeam DiscordTeam, Team Team, IEnumerable<int> BoardIndexes) : IRequest<Result>;