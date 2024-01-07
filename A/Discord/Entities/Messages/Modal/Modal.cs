// <copyright file="Modal.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DiscordLibrary.DiscordComponents;
using DSharpPlus.Entities;

public class Modal : InteractionMessage, IInteractable
{
    public string Title { get; }
    public string CustomId { get; }

    public Modal(string title, DiscordInteraction interaction, string customId = "") : base(interaction)
    {
        Title = title;
        CustomId = string.IsNullOrEmpty(customId) ? Guid.NewGuid().ToString() : customId;
    }

    internal override DiscordInteractionResponseBuilder GetInteractionResponseBuilder() =>
        base.GetInteractionResponseBuilder()
            .WithTitle(Title)
            .WithCustomId(CustomId);
}