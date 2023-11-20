// <copyright file="InteractionError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordEntities.Messages;

/// <inheritdoc/>
public abstract class InteractionError : HandlerError, IInteractionError
{
    public Message DiscordMessage { get; private set; }

    public InteractionError(Message message) : base(message.Content) =>
        DiscordMessage = message;

    public InteractionError(string message) : base(message) =>
        DiscordMessage = new Message().WithContent(message);
}