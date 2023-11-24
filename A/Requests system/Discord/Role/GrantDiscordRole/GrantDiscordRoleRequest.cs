// <copyright file="GrantDiscordRoleRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using MediatR;

internal record GrantDiscordRoleRequest(DiscordMember Member, DiscordRole Role) : IRequest<Result>;