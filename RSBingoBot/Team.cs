using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using RSBingoBot.Component_interaction_handlers;

namespace RSBingoBot;

internal class Team
{
    Team(string name, DiscordGuild guild)
    {
        Name = name;
        Guild = guild;
    }

    public string Name { get; private set; } = null!;
    public DiscordGuild Guild { get; private set; }
    public DiscordChannel BoardChannel { get; private set; } = null!;
    public DiscordChannel GeneralChannel { get; private set; } = null!;
    public DiscordChannel SubmittedEvidenceChannel { get; private set; } = null!;
    public DiscordChannel VoiceChannel { get; private set; } = null!;

    public static async Task<Team> CreateTeam(string name, DiscordGuild guild, bool preExisting)
    {
        Team team = new(name, guild);
        await team.Initialise(preExisting);
        return team;
    }

    async Task Initialise(bool preExisting)
    {
        if (preExisting)
        {
            foreach (DiscordChannel? channel in await Guild.GetChannelsAsync())
            {
                if (channel.Name == $"{Name}-board")
                {
                    BoardChannel = channel;
                }
                else if (channel.Name == $"{Name}-submitted-evidence")
                {
                    SubmittedEvidenceChannel = channel;
                }
            }
        }
        else
        {
            await CreateChannels();
            await InitialiseChannels();
        }

        await SetupChannels();
    }

    async Task CreateChannels()
    {
        DiscordChannel? parent = await Guild.CreateChannelAsync($"{Name}", ChannelType.Category);
        BoardChannel = await Guild.CreateChannelAsync($"{Name}-board", ChannelType.Text, parent);
        GeneralChannel = await Guild.CreateChannelAsync($"{Name}-general", ChannelType.Text, parent);
        SubmittedEvidenceChannel = await Guild.CreateChannelAsync($"{Name}-submitted-evidence", ChannelType.Text, parent);
        VoiceChannel = await Guild.CreateChannelAsync($"{Name}-voice", ChannelType.Voice, parent);
    }

    async Task InitialiseChannels()
    {
        await InitialiseBoardChannel();
    }

    async Task SetupChannels()
    {
        await SetupBoardChannel();
    }

    async Task InitialiseBoardChannel()
    {
        try
        {
            var changeTileButton = new DiscordButtonComponent(
                ButtonStyle.Primary,
                BoardChannel.Name + "_change_tile_button",
                "Change tile");

            var submitEvidenceButton = new DiscordButtonComponent(
                ButtonStyle.Primary,
                BoardChannel.Name + "_submit_evidence_button",
                "Submit evidence");

            var viewEvidenceButton = new DiscordButtonComponent(
                ButtonStyle.Primary,
                BoardChannel.Name + "_view_evidence_button",
                "View evidence");

            string imagePath = "E:/C#/Discord bots/RS-Bingo-Bot/RSBingoBot/";
            using var fs = new FileStream(imagePath + "Test board.png", FileMode.Open, FileAccess.Read);

            var builder = await new DiscordMessageBuilder()
                //.WithContent("Your board")
                .AddComponents(changeTileButton, submitEvidenceButton, viewEvidenceButton)
                .WithFiles(new Dictionary<string, Stream>() { { "Test_board.png", fs } })
                .SendAsync(BoardChannel);

            //var interactivity = Bot.Client.GetInteractivity();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    async Task SetupBoardChannel()
    {
        ComponentInteractionHandler.Register<ChangeTileButtonHanlder>(BoardChannel.Name + "_change_tile_button", this);
        ComponentInteractionHandler.Register<SubmitEvidenceButtonHandler>(BoardChannel.Name + "_submit_evidence_button", this);
        //ComponentInteractionHandler.Register(typeof(ChangeTileButtonHanlder), changeTileButton.CustomId);
    }
}
