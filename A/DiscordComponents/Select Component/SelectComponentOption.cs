// <copyright file="SelectComponentOption.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DSharpPlus.Entities;
using static RSBingo_Common.General;

public abstract class SelectComponentOption
{
    public string label { get; set; }
    public string? description { get; set; }
    public bool isDefault { get; set; }
    public DiscordComponentEmoji? emoji { get; set; }
    public DiscordSelectComponentOption discordOption { get; private set; }

    public SelectComponentOption(string label, string? description = null,
                bool isDefault = false, DiscordComponentEmoji? emoji = null)
    {
        this.label = label;
        this.description = description;
        this.isDefault = isDefault;
        this.emoji = emoji;
    }

    // TODO: JR - see if this can be built straight from the constructor.
    public void Build(string id)
    {
        discordOption = new(label, id, description!, isDefault, emoji!);
    }
}