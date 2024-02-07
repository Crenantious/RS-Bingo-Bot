// <copyright file="InteractionMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DSharpPlus.Entities;

public class InteractionMessage : Message
{
    public bool IsEphemeral { get; set; } = false;
    public DiscordInteraction Interaction { get; }
    public bool IsKeepAliveResponse { get; internal set; } = false;

    public InteractionMessage(DiscordInteraction interaction)
    {
        Interaction = interaction;
    }

    public InteractionMessage(DiscordInteraction interaction, DiscordMessage message) : base(message)
    {
        Interaction = interaction;
    }

    internal virtual DiscordInteractionResponseBuilder GetInteractionResponseBuilder()
    {
        var builder = GetBaseMessageBuilder(new DiscordInteractionResponseBuilder());
        builder.IsEphemeral = IsEphemeral;
        return builder;
    }

    internal DiscordFollowupMessageBuilder GetFollowupMessageBuilder()
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