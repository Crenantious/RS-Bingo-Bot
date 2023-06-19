// <copyright file="IInteraction.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Interfaces;

using MediatR;
using FluentResults;
using DSharpPlus.Entities;

public interface IInteractionRequest<TResponse> : IRequest<TResponse>, IValidatable
    where TResponse : Result
{
    public DiscordInteraction DiscordInteraction { get; set; }
}