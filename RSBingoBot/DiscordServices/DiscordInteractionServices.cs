// <copyright file="DiscordInteractionServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordServices;

using RSBingoBot.Interfaces;
using DSharpPlus;
using DSharpPlus.Entities;
using static DiscordRequestServices;

public class DiscordInteractionServices : IDiscordInteractionServices
{
    public async Task<bool> Respond(DiscordInteraction interaction, string content, bool isEphemeral) =>
        await Respond(interaction, new()
        {
            Content = content,
            IsEphemeral = isEphemeral
        });

    public async Task<bool> Respond(DiscordInteraction interaction, DiscordInteractionResponseBuilder builder) =>
        await SendDiscordRequest(async () => await interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder));

    public async Task<bool> EditOriginal(DiscordInteraction interaction, string content) =>
        await EditOriginal(interaction, new DiscordWebhookBuilder()
        {
            Content = content
        });

    public async Task<bool> EditOriginal(DiscordInteraction interaction, DiscordWebhookBuilder builder) =>
        await SendDiscordRequest(async () => await interaction.EditOriginalResponseAsync(builder));

    public async Task<bool> Followup(DiscordInteraction interaction, string content, bool isEphemeral) =>
        await Followup(interaction, new()
        {
            Content = content,
            IsEphemeral = isEphemeral
        });

    public async Task<bool> Followup(DiscordInteraction interaction, DiscordFollowupMessageBuilder builder) =>
        await SendDiscordRequest(async () => await interaction.CreateFollowupMessageAsync(builder));

    public async Task<bool> DeleteOriginalResponse(DiscordInteraction interaction) =>
        await SendDiscordRequest(async () => await interaction.DeleteOriginalResponseAsync());

    public async Task<bool> DeleteFollowup(DiscordInteraction interaction, ulong messageId) =>
        await SendDiscordRequest(async () => await interaction.DeleteFollowupMessageAsync(messageId));
}