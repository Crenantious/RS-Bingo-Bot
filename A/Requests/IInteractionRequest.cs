// <copyright file="IInteractionRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;
using FluentResults;
using MediatR;
using DiscordLibrary.RequestHandlers;

public interface IInteractionRequest : IRequest<Result>
{
    public IInteractionHandler? ParentHandler { get; protected set; }
    public DiscordInteraction Interaction { get; internal set; }
}