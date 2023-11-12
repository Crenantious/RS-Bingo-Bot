// <copyright file="RenameTeamRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using MediatR;

internal record RenameTeamRequest(string TeamName, string NewTeamName) :
    IRequest<Result>, IRequestWithTeamName, IRequestWithNewTeamName;