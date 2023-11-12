// <copyright file="CreateTeamRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using MediatR;

internal record CreateTeamRequest(string NewTeamName) : IRequest<Result>, IRequestWithNewTeamName;