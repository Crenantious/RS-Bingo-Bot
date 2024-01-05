// <copyright file="AddUserToTeamRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using MediatR;

public record AddUserToTeamRequest(DiscordUser User, RSBingoBot.Discord.DiscordTeam DiscordTeam) : IRequest<Result>;