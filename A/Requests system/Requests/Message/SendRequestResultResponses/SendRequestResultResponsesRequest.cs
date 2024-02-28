// <copyright file="SendRequestResultResponsesRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using FluentResults;
using MediatR;

public record SendRequestResultResponsesRequest(Message Response) : IRequest<Result>;