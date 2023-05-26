// <copyright file="DiscordInteractionServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordServices;

using DSharpPlus;
using DSharpPlus.Entities;
using static DiscordRequestServices;

public class DiscordInteractionServices
{
    public async Task<bool> Respond(DiscordInteraction interaction, string content, bool isEphemeral) =>
        await Respond(interaction, new()
        {
            Content = content,
            IsEphemeral = isEphemeral
        });

    public async Task<bool> Respond(DiscordInteraction interaction, DiscordInteractionResponseBuilder builder) =>
        await SendDiscordRequest(async () => await interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder));

    public async Task<bool> EditOriginal(DiscordInteraction interaction);
    public async Task<bool> EditOriginal(DiscordInteraction interaction, DiscordInteractionResponseBuilder builder);
    public async Task<bool> Followup(DiscordInteraction interaction);
    public async Task<bool> Followup(DiscordInteraction interaction, DiscordInteractionResponseBuilder builder);
    public async Task<bool> DeleteOriginal(DiscordInteraction interaction);
    public async Task<bool> DeleteOriginal(DiscordInteraction interaction, DiscordInteractionResponseBuilder builder);
    public async Task<bool> DelelteFollowup(DiscordInteraction interaction);
    public async Task<bool> DelelteFollowup(DiscordInteraction interaction, DiscordInteractionResponseBuilder builder);
}