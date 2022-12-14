// <copyright file="InitialiseTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot
{
    using DSharpPlus;
    using DSharpPlus.Entities;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using RSBingo_Framework.Records;
    using RSBingoBot.Component_interaction_handlers;
    using static RSBingo_Framework.DAL.DataFactory;

    /// <summary>
    /// Creates and sets up channels, roles and messages for the team.
    /// </summary>
    public class InitialiseTeam
    {
        private readonly DiscordClient discordClient;
        private readonly IDataWorker dataWorker = CreateDataWorker();

        private Team team = null!;
        private string changeTileButtonId = string.Empty!;
        private string submitEvidenceButtonId = string.Empty!;
        private string viewEvidenceButtonId = string.Empty!;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialiseTeam"/> class.
        /// </summary>
        /// <param name="discordClient">The <see cref="DiscordClient"/> the bot is using.</param>
        /// <param name="name">The team's name.</param>
        /// <param name="componentInteractionHandler">The handler to register component interactions with.</param>
        public InitialiseTeam(DiscordClient discordClient, string name)
        {
            Name = name;
            this.discordClient = discordClient;
        }

        /// <summary>
        /// The <see cref="InitialiseTeam"/>'s factory.
        /// </summary>
        /// <param name="name">The team's name.</param>
        /// <returns>The newly created <see cref="InitialiseTeam"/>.</returns>
        public delegate InitialiseTeam Factory(string name);

        /// <summary>
        /// Gets the name of the team.
        /// </summary>
        public string Name { get; private set; } = null!;

        /// <summary>
        /// Gets the team's board channel.
        /// </summary>
        public DiscordChannel BoardChannel { get; private set; } = null!;

        /// <summary>
        /// Creates and initializes the team's channels if they do not exist.
        /// </summary>
        /// <param name="preExisting">Weather or not the team has previously been created. (Remove with DB hookup.)</param>
        /// <param name="guild">The <see cref="DiscordClient"/> under which the team was created.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InitialiseAsync(bool preExisting, DiscordGuild? guild = null)
        {
            changeTileButtonId = Name + "_change_tile_button";
            submitEvidenceButtonId = Name + "_submit_evidence_button";
            viewEvidenceButtonId = Name + "_view_evidence_button";

            if (preExisting)
            {
                team = dataWorker.Teams.GetByName(Name);
                BoardChannel = await discordClient.GetChannelAsync(team.BoardChannelId);
            }
            else
            {
                await CreateChannels();
                await InitialiseChannels();
                CreateTeamEntry();
            }

            RegisterChannelComponentInteractions();
        }

        private void CreateTeamEntry()
        {
            dataWorker.Teams.Create(Name, BoardChannel.Id);
            dataWorker.SaveChanges();
        }

        private async Task CreateChannels()
        {
            DiscordChannel? category = await Guild.CreateChannelAsync($"{Name}", ChannelType.Category);
            BoardChannel = await Guild.CreateChannelAsync($"{Name}-board", ChannelType.Text, category);
            await Guild.CreateChannelAsync($"{Name}-general", ChannelType.Text, category);
            await Guild.CreateChannelAsync($"{Name}-submitted-evidence", ChannelType.Text, category);
            await Guild.CreateChannelAsync($"{Name}-voice", ChannelType.Voice, category);
        }

        private async Task InitialiseChannels()
        {
            await InitialiseBoardChannel();
        }

        private void RegisterChannelComponentInteractions()
        {
            RegisterBoardChannelComponentInteractions();
        }

        private async Task InitialiseBoardChannel()
        {
            try
            {
                var changeTileButton = new DiscordButtonComponent(
                    ButtonStyle.Primary,
                    changeTileButtonId,
                    "Change tile");

                var submitEvidenceButton = new DiscordButtonComponent(
                    ButtonStyle.Primary,
                    submitEvidenceButtonId,
                    "Submit evidence");

                var viewEvidenceButton = new DiscordButtonComponent(
                    ButtonStyle.Primary,
                    viewEvidenceButtonId,
                    "View evidence");

                string imagePath = "E:/C#/Discord bots/RS-Bingo-Bot/RSBingoBot/";
                using var fs = new FileStream(imagePath + "Test board.png", FileMode.Open, FileAccess.Read);

                var builder = await new DiscordMessageBuilder()
                    .AddComponents(changeTileButton, submitEvidenceButton, viewEvidenceButton)
                    .WithFiles(new Dictionary<string, Stream>() { { "Test_board.png", fs } })
                    .SendAsync(BoardChannel);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e);
            }
        }

        private void RegisterBoardChannelComponentInteractions()
        {
            ComponentInteractionHandler.InitialisationInfo info = new ()
            {
                Team = this,
            };

            ComponentInteractionHandler.Register<ChangeTileButtonHanlder>(changeTileButtonId, info);
            ComponentInteractionHandler.Register<SubmitEvidenceButtonHandler>(submitEvidenceButtonId, info);
            //ComponentInteractionHandler.Register<ViewEvidenceButtonHandler>(viewEvidenceButtonId, info);
        }
    }
}