// <copyright file="InteractionWarning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordEntities.Messages;

/// <inheritdoc/>
public abstract class InteractionWarning : HandlerWarning, IInteractionWarning, IInteractionReason
{
    public InteractionWarning(Message message) : base(message.Content) =>
        WithMetadata(IInteractionReason.DiscordMessageMetaDataKey, message);

    public InteractionWarning(string message) : base(message) =>
        WithMetadata(IInteractionReason.DiscordMessageMetaDataKey, new Message().WithContent(message));
}