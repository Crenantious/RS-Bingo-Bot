// <copyright file="SelectComponentItem.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DSharpPlus.Entities;

// TODO: JR - find a way to make this take a generic as the type for value.
public class SelectComponentItem : SelectComponentOption
{
    public object? Value { get; }

    public SelectComponentItem(string label, object? value, string? description = null,
        bool isDefault = false, DiscordComponentEmoji? emoji = null) : base(label, description, isDefault, emoji)
    {
        Value = value;
    }
}