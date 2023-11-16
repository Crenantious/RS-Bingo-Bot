// <copyright file="InteractionSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordEntities.Messages;

/// <inheritdoc/>
public abstract class InteractionSuccess : Success, IInteractionSuccess, IInteractionReason
{
    public InteractionSuccess(Message message) : base(message.Content) =>
        WithMetadata(IInteractionReason.DiscordMessageMetaDataKey, message);

    public InteractionSuccess(string message) : base(message) =>
        WithMetadata(IInteractionReason.DiscordMessageMetaDataKey, new Message().WithContent(message));
}