﻿// <copyright file="InteractionMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DSharpPlus.Entities;

public class InteractionMessage : Message
{
    internal ulong FollowupMessageId { get; set; } = 0;

    public bool IsEphemeral { get; set; }
    public DiscordInteraction Interaction { get; }

    public InteractionMessage(DiscordInteraction interaction)
    {
        Interaction = interaction;
    }

    public virtual DiscordInteractionResponseBuilder GetInteractionResponseBuilder()
    {
        var builder = GetBaseMessageBuilder(new DiscordInteractionResponseBuilder());
        builder.IsEphemeral = IsEphemeral;
        return builder;
    }

    public DiscordFollowupMessageBuilder GetFollowupMessageBuilder()
    {
        var builder = GetBaseMessageBuilder(new DiscordFollowupMessageBuilder());
        builder.IsEphemeral = IsEphemeral;
        return builder;
    }

    public static InteractionMessage operator +(InteractionMessage prefix, InteractionMessage suffix) =>
        (InteractionMessage)((prefix as Message) + (suffix as Message));

    public static InteractionMessage operator +(InteractionMessage prefix, Message suffix) =>
        (InteractionMessage)((prefix as Message) + suffix);
}