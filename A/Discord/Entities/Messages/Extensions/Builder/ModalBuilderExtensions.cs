// <copyright file="ModalBuilderExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DSharpPlus.Entities;

public static class ModalBuilderExtensions
{
    internal static DiscordInteractionResponseBuilder GetInteractionResponseBuilder(this Modal modal) =>
        InteractionMessageBuilderExtensions.GetInteractionResponseBuilder(modal)
            .WithTitle(modal.Title)
            .WithCustomId(modal.CustomId);
}