// <copyright file="SendMessageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using FluentResults;
using MediatR;

internal record SendMessageRequest(Message Message, DiscordChannel Channel) : IRequest<Result>;