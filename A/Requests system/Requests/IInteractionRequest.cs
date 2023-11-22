// <copyright file="IInteractionRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using MediatR;

public interface IInteractionRequest : IRequest<Result>
{
    /// <summary>
    /// Value will be set by the framework.
    /// </summary>
    public InteractionArgs InteractionArgs { get; set; }
}