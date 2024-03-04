// <copyright file="InteractionMessageBuilderExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DSharpPlus.Entities;

public static class InteractionMessageBuilderExtensions
{
    internal static DiscordInteractionResponseBuilder GetInteractionResponseBuilder(this InteractionMessage message)
    {
        var builder = message.GetBaseMessageBuilder(new DiscordInteractionResponseBuilder());
        builder.IsEphemeral = message.IsEphemeral;
        return builder;
    }

    internal static DiscordFollowupMessageBuilder GetFollowupMessageBuilder(this InteractionMessage message)
    {
        var builder = message.GetBaseMessageBuilder(new DiscordFollowupMessageBuilder());
        builder.IsEphemeral = message.IsEphemeral;
        return builder;
    }
}