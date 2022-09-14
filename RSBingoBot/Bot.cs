using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Entities;
using RSBingoBot.Slash_commands;

namespace RSBingoBot;

public class Bot
{
    readonly string token = Environment.GetEnvironmentVariable("BOT_TOKEN")!;

    readonly string[] prefixes = new string[] { "." };
    readonly List<Team> teams = new();

    public static DiscordClient Client { get; private set; } = null!;
    public static InteractivityExtension Interactivity { get; private set; } = null!;
    public static CommandsNextExtension CommandsNext { get; private set; } = null!;
    public static SlashCommandsExtension SlashCommands { get; private set; } = null!;

    public async Task RunAsync()
    {
        Console.WriteLine(token);
        Client = new DiscordClient(new DiscordConfiguration()
        {
            Token = token,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged
        });

        Interactivity = Client.UseInteractivity(new InteractivityConfiguration { });

        CommandsNext = Client.UseCommandsNext(new CommandsNextConfiguration
        {
            StringPrefixes = prefixes,
            EnableDms = false,
            EnableDefaultHelp = true,
            DmHelp = false
        });

        SlashCommands = Client.UseSlashCommands(new SlashCommandsConfiguration { });
        SlashCommands.RegisterCommands<TeamCommands>();

        Client.Ready += OnClientReady;

        await Client.ConnectAsync();
        await Task.Delay(-1);
    }

    async Task OnClientReady(DiscordClient client, ReadyEventArgs args)
    {
        Console.WriteLine("Bot ready");
        foreach(DiscordGuild? guild in client.Guilds.Values)
        {
            await Team.CreateTeam("test", guild, true);
        }
    }
}