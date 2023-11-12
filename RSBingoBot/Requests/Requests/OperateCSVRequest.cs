// <copyright file="OperateCSVRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using MediatR;

internal record OperateCSVRequest(DiscordAttachment Attachment) : IRequest<Result>, IRequestWithDiscordAttachment;