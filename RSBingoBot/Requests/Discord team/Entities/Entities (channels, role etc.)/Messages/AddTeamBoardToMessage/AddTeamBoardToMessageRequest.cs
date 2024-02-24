// <copyright file="AddTeamBoardToMessageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using FluentResults;
using MediatR;
using RSBingo_Framework.Models;

internal record AddTeamBoardToMessageRequest(Team Team, Message Message) : IRequest<Result>;