﻿// <copyright file="MessageUtilities.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

internal static class InteractionMessageUtilities
{
    public static async Task Respond(InteractionCreateEventArgs args, string content, bool isEphemeral)
    {
        var builder = new DiscordInteractionResponseBuilder()
            .WithContent(content)
            .AsEphemeral();

        await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
    }

    public static async Task EditResponse(InteractionCreateEventArgs args, string content, bool isEphemeral)
    {
        var builder = new DiscordWebhookBuilder()
            .WithContent(content);

        await args.Interaction.EditOriginalResponseAsync(builder);
    }

    public static async Task Followup(InteractionCreateEventArgs args, string content, bool isEphemeral)
    {
        var builder = new DiscordFollowupMessageBuilder()
            .WithContent(content)
            .AsEphemeral();

        await args.Interaction.CreateFollowupMessageAsync(builder);
    }

    public static async Task EditFollowup(InteractionCreateEventArgs args, ulong messageId, string content, bool isEphemeral)
    {
        var builder = new DiscordWebhookBuilder()
            .WithContent(content);

        await args.Interaction.EditFollowupMessageAsync(messageId, builder);
    }
}