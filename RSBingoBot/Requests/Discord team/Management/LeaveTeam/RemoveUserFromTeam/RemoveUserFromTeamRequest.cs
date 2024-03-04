// <copyright file="RemoveUserFromTeamRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using MediatR;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;

public record RemoveUserFromTeamRequest(IDataWorker DataWorker, DiscordMember Member,
    User User, RSBingoBot.Discord.DiscordTeam DiscordTeam) : IRequest<Result>;