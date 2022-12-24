using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace RSBingoBot.Component_interaction_handlers.Select_Component
{
    public class SelectComponentPage : SelectComponentOption
    {
        public List<SelectComponentOption> Options = new();

        public SelectComponentPage(string label, string? description = null,
            bool isDefault = false, DiscordComponentEmoji? emoji = null) :
            base(label, description, isDefault, emoji) { }
    }
}
