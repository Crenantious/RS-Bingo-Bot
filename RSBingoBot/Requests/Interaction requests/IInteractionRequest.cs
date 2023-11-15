// <copyright file="IInteractionRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using MediatR;
using RSBingoBot.InteractionHandlers;

internal interface IInteractionRequest : IRequest<Result>
{
    public IInteractionHandler? ParentHandler { get; }

    /// <summary>
    /// Value will be set when the request is being processed.
    /// </summary>
    // TODO: JR - put the base requests, handlers and validators in their own assembly and make this internal.
    public DiscordInteraction Interaction { get; set; }
}