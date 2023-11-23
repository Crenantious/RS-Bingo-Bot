// <copyright file="CreateTeamCategoryChannelRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using MediatR;
using RSBingo_Framework.Models;

internal record CreateTeamCategoryChannelRequest(Team Team) : IRequest<Result>;