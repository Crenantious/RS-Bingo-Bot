// <copyright file="CreateTeamBoardMessageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using FluentResults;
using MediatR;
using RSBingoBot.Discord;

internal record CreateTeamBoardMessageRequest(DiscordTeam DiscordTeam) : IRequest<Result<Message>>;