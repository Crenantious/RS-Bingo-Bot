using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

namespace BingoBotEmbed
{
    public class Bot
    {
        const string token = "";

        readonly string[] prefixes = new string[] { "." };

        public static DiscordClient Client { get; private set; } = null!;
        public static InteractivityExtension Interactivity { get; private set; } = null!;
        public static CommandsNextExtension Commands { get; private set; } = null!;

        public async Task RunAsync()
        {
            Client = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });

            Client.UseInteractivity(new InteractivityConfiguration { });

            Commands = Client.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = prefixes,
                EnableDms = false,
                EnableDefaultHelp = true,
                DmHelp = false
            });

            SlashCommandsExtension? slash = Client.UseSlashCommands();

            Client.Ready += OnClientReady;

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        Task OnClientReady(DiscordClient client, ReadyEventArgs args)
        {
            Console.WriteLine("Bot ready");
            return Task.CompletedTask;
        }
    }
}