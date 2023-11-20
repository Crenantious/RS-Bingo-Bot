// <copyright file="InteractionSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordEntities.Messages;

/// <inheritdoc/>
public abstract class InteractionSuccess : HandlerSuccess, IInteractionSuccess
{
    public Message DiscordMessage { get; private set; }

    public InteractionSuccess(Message message) : base(message.Content) =>
        DiscordMessage = message;

    public InteractionSuccess(string message) : base(message) =>
        DiscordMessage = new Message().WithContent(message);
}