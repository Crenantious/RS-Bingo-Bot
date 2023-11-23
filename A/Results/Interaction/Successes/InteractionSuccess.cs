// <copyright file="InteractionSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

/// <inheritdoc/>
public abstract class InteractionSuccess : HandlerSuccess, IInteractionSuccess
{
    public InteractionMessage InteractionMessage { get; private set; }

    public InteractionSuccess(InteractionMessage message) : base(message.Content) =>
        InteractionMessage = message;

    public InteractionSuccess(string message, DiscordInteraction interaction) : base(message) =>
        InteractionMessage = new InteractionMessage(interaction).WithContent(message);
}