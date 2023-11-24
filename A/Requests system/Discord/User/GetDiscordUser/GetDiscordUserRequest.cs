// <copyright file="GetDiscordUserRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using MediatR;

internal record GetDiscordUserRequest(ulong Id) : IRequest<Result<DiscordMember>>;