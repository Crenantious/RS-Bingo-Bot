// <copyright file="UpdateLeaderboardRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using FluentResults;
using MediatR;

internal record UpdateLeaderboardRequest(Message? Message) : IRequest<Result>;