// <copyright file="AssignTeamRoleRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using MediatR;
using RSBingo_Framework.Models;

internal record AssignTeamRoleRequest(Team Team, DiscordRole Role) : IRequest<Result>;