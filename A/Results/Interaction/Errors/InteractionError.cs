// <copyright file="InteractionError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

/// <inheritdoc/>
public abstract class InteractionError : HandlerError, IInteractionError
{
    public InteractionMessage InteractionMessage { get; private set; }

    public InteractionError(InteractionMessage message) : base(message.Content) =>
        InteractionMessage = message;

    public InteractionError(string message, DiscordInteraction interaction) : base(message) =>
        InteractionMessage = new InteractionMessage(interaction).WithContent(message);

}