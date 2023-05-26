// <copyright file="IDiscordInteractionServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Interfaces;

using DSharpPlus.Entities;

public interface IDiscordInteractionServices
{
    public Task<bool> Respond(DiscordInteraction interaction, string content, bool isEphemeral);
    public Task<bool> Respond(DiscordInteraction interaction, DiscordInteractionResponseBuilder builder);
    public Task<bool> EditOriginal(DiscordInteraction interaction);
    public Task<bool> EditOriginal(DiscordInteraction interaction, DiscordInteractionResponseBuilder builder);
    public Task<bool> Followup(DiscordInteraction interaction);
    public Task<bool> Followup(DiscordInteraction interaction, DiscordInteractionResponseBuilder builder);
    public Task<bool> DeleteOriginal(DiscordInteraction interaction);
    public Task<bool> DeleteOriginal(DiscordInteraction interaction, DiscordInteractionResponseBuilder builder);
    public Task<bool> DelelteFollowup(DiscordInteraction interaction);
    public Task<bool> DelelteFollowup(DiscordInteraction interaction, DiscordInteractionResponseBuilder builder);
}