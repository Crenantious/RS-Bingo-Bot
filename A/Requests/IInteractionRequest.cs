// <copyright file="IInteractionRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.RequestHandlers;
using DSharpPlus.EventArgs;
using FluentResults;
using MediatR;

public interface IInteractionRequest : IRequest<Result>
{
    internal ComponentInteractionCreateEventArgs InteractionArgs { get; }
    public IInteractionHandler? ParentHandler { get; protected set; }
}