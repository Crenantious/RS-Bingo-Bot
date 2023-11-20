// <copyright file="InteractionWarning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordEntities.Messages;

/// <inheritdoc/>
public abstract class InteractionWarning : HandlerWarning, IInteractionWarning
{
    public Message DiscordMessage { get; private set; }

    public InteractionWarning(Message message) : base(message.Content) =>
        DiscordMessage = message;

    public InteractionWarning(string message) : base(message) =>
        DiscordMessage = new Message().WithContent(message);
}