// <copyright file="SelectComponentPage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordComponents;

using DSharpPlus.Entities;

public class SelectComponentPage : SelectComponentOption
{
    public List<SelectComponentOption> Options = new();

    public SelectComponentPage(string label, string? description = null,
        bool isDefault = false, DiscordComponentEmoji? emoji = null) :
        base(label, description, isDefault, emoji) { }
}