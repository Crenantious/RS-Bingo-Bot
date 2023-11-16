// <copyright file="InteractionError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordEntities.Messages;

/// <inheritdoc/>
public abstract class InteractionError : Error, IInteractionError, IInteractionReason
{
    public InteractionError(Message message) : base(message.Content) =>
        WithMetadata(IInteractionReason.DiscordMessageMetaDataKey, message);

    public InteractionError(string message) : base(message) =>
        WithMetadata(IInteractionReason.DiscordMessageMetaDataKey, new Message().WithContent(message));
}