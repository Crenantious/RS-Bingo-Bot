// <copyright file="SelectComponentItem.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DSharpPlus.Entities;

public class SelectComponentItem : SelectComponentOption
{
    public object? value { get; }

    public SelectComponentItem(string label, object? value, string? description = null,
        bool isDefault = false, DiscordComponentEmoji? emoji = null) : base(label, description, isDefault, emoji)
    {
        this.value = value;
    }
}