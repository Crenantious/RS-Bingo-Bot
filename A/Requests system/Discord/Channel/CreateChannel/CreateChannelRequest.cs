// <copyright file="CreateChannelRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus;
using DSharpPlus.Entities;
using FluentResults;
using MediatR;

internal record CreateChannelRequest(string Name, ChannelType ChannelType, DiscordChannel? Parent = null,
        IEnumerable<DiscordOverwriteBuilder>? Overwrites = null) : IRequest<Result<DiscordChannel>>;