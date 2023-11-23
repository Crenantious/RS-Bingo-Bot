// <copyright file="InteractionWarning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

/// <inheritdoc/>
public abstract class InteractionWarning : HandlerWarning, IInteractionWarning
{
    public InteractionMessage InteractionMessage { get; private set; }

    public InteractionWarning(InteractionMessage message) : base(message.Content) =>
        InteractionMessage = message;

    public InteractionWarning(string message, DiscordInteraction interaction) : base(message) =>
        InteractionMessage = new InteractionMessage(interaction).WithContent(message);
}