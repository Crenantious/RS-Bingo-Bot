// <copyright file="IInteractionResponseOverride.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using MediatR;

/// <summary>
/// Attach this to an <see cref="IRequest"/> to force <see cref="IDiscordResponse"/>s to be sent in
/// response to <see cref="IInteractionResponseOverride.ResponseOverride"/> instead of the <see cref="DiscordInteraction"/>
/// it would have normally.
/// </summary>
public interface IInteractionResponseOverride : IRequestResponse
{
    /// <summary>
    /// This should be an empty message as the responses will be automatically added to it.
    /// </summary>
    public InteractionMessage ResponseOverride { get; }
}