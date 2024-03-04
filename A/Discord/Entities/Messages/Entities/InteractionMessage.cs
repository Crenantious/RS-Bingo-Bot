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

    public InteractionMessage(DiscordInteraction interaction) : base(interaction.Channel)
    {
        Interaction = interaction;
    }

    public static InteractionMessage operator +(InteractionMessage prefix, InteractionMessage suffix) =>
        (InteractionMessage)((prefix as Message) + (suffix as Message));

    public static InteractionMessage operator +(InteractionMessage prefix, Message suffix) =>
        (InteractionMessage)((prefix as Message) + suffix);
}