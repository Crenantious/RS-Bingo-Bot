﻿// <copyright file="CreateTeamCategoryChannelRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using MediatR;

internal record CreateTeamCategoryChannelRequest(RSBingoBot.Discord.DiscordTeam DiscordTeam) : IRequest<Result<DiscordChannel>>;