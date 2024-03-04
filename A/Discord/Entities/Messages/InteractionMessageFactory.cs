// <copyright file="MessageFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DSharpPlus.Entities;

public class InteractionMessageFactory
{
    public InteractionMessage Create(DiscordInteraction interaction) =>
        new(interaction);
}