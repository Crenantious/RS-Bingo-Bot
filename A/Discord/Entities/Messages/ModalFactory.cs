// <copyright file="ModalFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DSharpPlus.Entities;

public class ModalFactory
{
    public Modal Create(string title, DiscordInteraction interaction, string customId = "") =>
        new(title, interaction, customId);
}