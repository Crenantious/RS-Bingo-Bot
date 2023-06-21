// <copyright file="InteractionMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordEntities;

using DSharpPlus.Entities;
using RSBingoBot.Interfaces;

public class InteractionMessage : Message
{
    public bool IsEphemeral { get; set; }

    public InteractionMessage() { }

    public InteractionMessage(string content) : base(content) { }

    /// <summary>
    /// Sets <see cref="IsEphemeral"/> to <paramref name="status"/>.
    /// </summary>
    public InteractionMessage AsEphemeral(bool status)
    {
        IsEphemeral = status;
        return this;
    }

    public DiscordInteractionResponseBuilder GetInteractionResponseBuilder()
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
}