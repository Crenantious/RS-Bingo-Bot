// <copyright file="PostTeamSignUpChannelMessageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using MediatR;

internal record PostTeamSignUpChannelMessageRequest(DiscordChannel Channel) : IRequest<Result>;