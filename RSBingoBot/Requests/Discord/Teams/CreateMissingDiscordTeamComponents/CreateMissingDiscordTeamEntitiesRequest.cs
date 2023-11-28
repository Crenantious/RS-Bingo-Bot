﻿// <copyright file="CreateMissingDiscordTeamEntitiesRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using MediatR;
using RSBingoBot.DTO;

internal record CreateMissingDiscordTeamEntitiesRequest(DiscordTeam DiscordTeam) : IRequest<Result>;