// <copyright file="CreateTeamVoiceChannelRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using MediatR;
using RSBingoBot.Discord;

internal record CreateTeamVoiceChannelRequest(DiscordTeam DiscordTeam) : IRequest<Result>;