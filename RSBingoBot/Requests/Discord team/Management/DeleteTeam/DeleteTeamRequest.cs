﻿// <copyright file="DeleteTeamRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using MediatR;
using RSBingoBot.Discord;

public record DeleteTeamRequest(DiscordTeam DiscordTeam) : IRequest<Result>;