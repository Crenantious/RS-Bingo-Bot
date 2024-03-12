// <copyright file="UpdateTeamBoardMessageButtonsRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using FluentResults;
using MediatR;

internal record UpdateTeamBoardMessageButtonsRequest(Message Message) : IRequest<Result>;