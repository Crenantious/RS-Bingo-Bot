﻿// <copyright file="GetRoleRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using MediatR;

internal record GetRoleRequest(ulong Id) : IRequest<Result<DiscordRole>>;