// <copyright file="CreateTeamBoardChannelRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using MediatR;
using RSBingo_Framework.Models;

internal record CreateTeamBoardChannelRequest(Team Team, DiscordChannel Category, DiscordRole TeamRole) : IRequest<Result>;