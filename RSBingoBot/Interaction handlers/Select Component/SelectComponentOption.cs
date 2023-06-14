using System;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RSBingo_Common.General;

namespace RSBingoBot.Component_interaction_handlers.Select_Component;

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

    public void Build(string id)
    {
        discordOption = new(label, id, description, isDefault, emoji);
    }
}
