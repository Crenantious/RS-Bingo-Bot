using System;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSBingoBot.Component_interaction_handlers.Select_Component
{
    public class SelectComponentItem : SelectComponentOption
    {
        public object? value { get; }

        public SelectComponentItem(string label, object? value, string? description = null,
            bool isDefault = false, DiscordComponentEmoji? emoji = null) : base(label, description, isDefault, emoji)
        {
            this.value = value;
        }
    }
}
