// <copyright file="CreateTeamBoardChannelRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using MediatR;
using RSBingoBot.Discord;

internal record CreateTeamBoardChannelRequest(DiscordTeam DiscordTeam) : IRequest<Result>;