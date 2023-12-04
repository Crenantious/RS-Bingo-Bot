// <copyright file="IInteractionRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.EventArgs;
using FluentResults;
using MediatR;

public interface IInteractionRequest<TArgs> : IRequest<Result>
    where TArgs : InteractionCreateEventArgs
{
    /// <summary>
    /// Value will be set by the framework.
    /// </summary>
    public TArgs InteractionArgs { get; set; }
}