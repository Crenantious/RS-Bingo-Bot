using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSBingoBot.Slash_commands
{
    internal class TeamCommands : ApplicationCommandModule
    {
        [SlashCommand("CreateTeam", "Creates a new team named test.")]
        public async Task<Team> CreateTeam(InteractionContext ctx)
        {
            return await Team.CreateTeam("Test", ctx.Guild, false);
        }
    }
}
