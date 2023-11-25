// <copyright file="GetMessageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using FluentResults;
using MediatR;

internal record GetMessageRequest(ulong Id, DiscordChannel Channel) : IRequest<Result<Message>>;