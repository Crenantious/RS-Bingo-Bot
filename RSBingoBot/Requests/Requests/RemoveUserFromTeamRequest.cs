// <copyright file="RemoveUserFromTeamRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using MediatR;

internal record RemoveUserFromTeamRequest(DiscordUser DiscordUser, string TeamName) :
    IRequest<Result>, IRequestWithDiscordUser, IRequestWithTeamName;