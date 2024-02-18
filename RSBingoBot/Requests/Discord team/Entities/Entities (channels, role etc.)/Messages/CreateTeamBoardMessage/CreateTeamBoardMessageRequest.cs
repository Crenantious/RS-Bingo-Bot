// <copyright file="CreateTeamBoardMessageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using FluentResults;
using MediatR;
using RSBingo_Framework.Models;
using RSBingoBot.Discord;

public record CreateTeamBoardMessageRequest(DiscordTeam DiscordTeam, Team Team) : IRequest<Result<Message>>;