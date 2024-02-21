// <copyright file="SelectComponentOption.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DSharpPlus.Entities;

public abstract class SelectComponentOption
{
    public abstract string Label { get; }
    public string? Description { get; }
    public bool IsDefault { get; internal set; }
    public DiscordComponentEmoji? Emoji { get; }

    /// <summary>
    /// Only set once <see cref="Build(string)"/> has been called.
    /// </summary>
    public DiscordSelectComponentOption DiscordOption { get; private set; } = null!;

    public SelectComponentOption(string? description = null,
                bool isDefault = false, DiscordComponentEmoji? emoji = null)
    {
        this.Description = description;
        this.IsDefault = isDefault;
        this.Emoji = emoji;
    }

    public void Build(string id)
    {
        DiscordOption = new(Label, id, Description!, IsDefault, Emoji!);
    }
}