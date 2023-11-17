// <copyright file="IInteractionRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.RequestHandlers;
using FluentResults;
using MediatR;

public interface IInteractionRequest : IRequest<Result>
{
    public IInteractionHandler? ParentHandler { get; }
}