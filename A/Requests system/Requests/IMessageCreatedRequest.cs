// <copyright file="IMessageCreatedRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.EventArgs;
using FluentResults;
using MediatR;

public interface IMessageCreatedRequest : IRequest<Result>
{
    public MessageCreateEventArgs MessageArgs { get; set; }
    public Message Message { get; set; }
}