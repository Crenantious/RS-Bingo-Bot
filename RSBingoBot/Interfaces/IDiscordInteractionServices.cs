﻿// <copyright file="IDiscordInteractionServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Interfaces;

using DSharpPlus.Entities;

public interface IDiscordInteractionServices
{
    public Task<bool> Respond(DiscordInteraction interaction, string content, bool isEphemeral);
    public Task<bool> Respond(DiscordInteraction interaction, DiscordInteractionResponseBuilder builder);
    public Task<bool> EditOriginal(DiscordInteraction interaction, string content);
    public Task<bool> EditOriginal(DiscordInteraction interaction, DiscordWebhookBuilder builder);
    public Task<bool> Followup(DiscordInteraction interaction, string content, bool isEphemeral);
    public Task<bool> Followup(DiscordInteraction interaction, DiscordFollowupMessageBuilder builder);
    public Task<bool> DeleteOriginalResponse(DiscordInteraction interaction);
    public Task<bool> DeleteFollowup(DiscordInteraction interaction, ulong messageId);
}