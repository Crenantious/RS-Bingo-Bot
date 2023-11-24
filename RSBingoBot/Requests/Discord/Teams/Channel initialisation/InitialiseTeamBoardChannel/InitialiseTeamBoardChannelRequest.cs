// <copyright file="InitialiseTeamBoardChannelRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using FluentResults;
using MediatR;
using RSBingo_Framework.Models;

internal record InitialiseTeamBoardChannelRequest(Team team, DiscordChannel BoardChannel) : IRequest<Result<Message>>;