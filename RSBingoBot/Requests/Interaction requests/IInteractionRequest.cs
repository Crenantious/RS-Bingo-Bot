// <copyright file="IInteractionRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using MediatR;
using RSBingoBot.InteractionHandlers;
using RSBingoBot.Interfaces;

internal interface IInteractionRequest : IRequest<Result>
{
    public IInteractionHandler? ParentHandler { get; }

    /// <summary>
    /// Value will be set when the <see cref="IInteractable"/> is interacted with. Do not set it otherwise.
    /// </summary>
    public DiscordInteraction Interaction { get; set; }
}