// <copyright file="IDiscordInteractionServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Interfaces;

using DSharpPlus.Entities;

public interface IDiscordInteractionServices
{
    public Task<bool> Respond(IDiscordInteraction interaction, string content, bool isEphemeral);
    public Task<bool> Respond(IDiscordInteraction interaction, DiscordInteractionResponseBuilder builder);
    public Task<bool> EditOriginal(IDiscordInteraction interaction);
    public Task<bool> EditOriginal(IDiscordInteraction interaction, DiscordInteractionResponseBuilder builder);
    public Task<bool> Followup(IDiscordInteraction interaction);
    public Task<bool> Followup(IDiscordInteraction interaction, DiscordInteractionResponseBuilder builder);
    public Task<bool> DeleteOriginal(IDiscordInteraction interaction);
    public Task<bool> DeleteOriginal(IDiscordInteraction interaction, DiscordInteractionResponseBuilder builder);
    public Task<bool> DelelteFollowup(IDiscordInteraction interaction);
    public Task<bool> DelelteFollowup(IDiscordInteraction interaction, DiscordInteractionResponseBuilder builder);
}