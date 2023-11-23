// <copyright file="CreateTeamVoiceChannelRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using MediatR;
using RSBingo_Framework.Models;

internal record CreateTeamVoiceChannelRequest(Team Team, DiscordChannel Category) : IRequest<Result>;