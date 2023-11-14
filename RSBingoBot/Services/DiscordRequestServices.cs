// <copyright file="DiscordInteractionServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordServices;

using DSharpPlus;
using DSharpPlus.Entities;
using RSBingoBot.DiscordEntities;
using RSBingoBot.Interfaces;

public class DiscordRequestServices : IDiscordRequestServices
{
    private async Task<bool> SendRequest(Func<Task> request)
    {
        try
        {
            await request();
            return true;
        }
        catch { return false; }
    }

    public async Task<bool> Respond(DiscordInteraction interaction, string content, bool isEphemeral) =>
        await Respond(interaction, new()
        {
            Content = content,
            IsEphemeral = isEphemeral
        });

    public async Task<bool> Respond(DiscordInteraction interaction, DiscordInteractionResponseBuilder builder) =>
        await SendRequest(async () => await interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder));

    public async Task<bool> EditOriginal(DiscordInteraction interaction, string content) =>
        await EditOriginal(interaction, new DiscordWebhookBuilder()
        {
            Content = content
        });

    public async Task<bool> EditOriginal(DiscordInteraction interaction, DiscordWebhookBuilder builder) =>
        await SendRequest(async () => await interaction.EditOriginalResponseAsync(builder));

    public async Task<bool> Followup(DiscordInteraction interaction, string content, bool isEphemeral) =>
        await Followup(interaction, new()
        {
            Content = content,
            IsEphemeral = isEphemeral
        });

    public async Task<bool> Followup(DiscordInteraction interaction, DiscordFollowupMessageBuilder builder) =>
        await SendRequest(async () => await interaction.CreateFollowupMessageAsync(builder));

    public async Task<bool> DeleteOriginalResponse(DiscordInteraction interaction) =>
        await SendRequest(async () => await interaction.DeleteOriginalResponseAsync());

    public async Task<bool> DeleteFollowup(DiscordInteraction interaction, ulong messageId) =>
        await SendRequest(async () => await interaction.DeleteFollowupMessageAsync(messageId));

    /// <summary>
    /// Sends a message to Discord in response to an interaction.
    /// </summary>
    /// <param name="hasBeenResponed">If <see langword="true"/>, the message will be send as a follow-up.
    /// Otherwise it will be sent as an original response.</param>
    /// <returns>If the message was sent successfully.</returns>
    public async Task<bool> SendMessage(DiscordInteraction interaction, InteractionMessage message, bool hasBeenResponed = true)
    {
        if (hasBeenResponed)
        {
            return await Followup(interaction, message.GetFollowupMessageBuilder());
        }
        return await Respond(interaction, message.GetInteractionResponseBuilder());
    }
}