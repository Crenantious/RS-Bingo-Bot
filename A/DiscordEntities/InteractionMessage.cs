﻿// <copyright file="InteractionMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DSharpPlus.Entities;

public class InteractionMessage : Message
{
    public bool IsEphemeral { get; set; }

    public InteractionMessage() { }

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