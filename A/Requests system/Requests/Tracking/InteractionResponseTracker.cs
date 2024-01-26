// <copyright file="InteractionResponseTracker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

public class InteractionResponseTracker
{
    private Dictionary<ulong, InteractionMessage> responses = new();

    internal void Register(DiscordInteraction interaction, InteractionMessage message)
    {
        responses[interaction.Id] = message;
    }

    public bool HasResponse(DiscordInteraction interaction) =>
        responses.ContainsKey(interaction.Id);
}