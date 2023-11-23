﻿// <copyright file="CreateTeamEvidenceChannelRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using MediatR;
using RSBingo_Framework.Models;

internal record CreateTeamEvidenceChannelRequest(Team Team) : IRequest<Result>;