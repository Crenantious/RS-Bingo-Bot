// <copyright file="DiscordTeam.cs" company="PlaceholderCompany">
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
    using SixLabors.ImageSharp.Processing;
    using SixLabors.ImageSharp;

    using static RSBingo_Framework.DAL.DataFactory;
    using RSBingoBot.Imaging;

    /// <summary>
    /// Creates and sets up channels, roles and messages for the team.
    /// </summary>
    public class DiscordTeam
    {
        private static Dictionary<int, DiscordTeam> instances = new();

        private readonly DiscordClient discordClient;
        private readonly IDataWorker dataWorker = CreateDataWorker();

        private Team team = null!;
        private string changeTileButtonId = string.Empty!;
        private string submitEvidenceButtonId = string.Empty!;
        private string viewEvidenceButtonId = string.Empty!;
        private DiscordMessage boardMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscordTeam"/> class.
        /// </summary>
        /// <param name="discordClient">The <see cref="DiscordClient"/> the bot is using.</param>
        /// <param name="name">The team's name.</param>
        /// <param name="componentInteractionHandler">The handler to register component interactions with.</param>
        public DiscordTeam(DiscordClient discordClient, string name)
        {
            Name = name;
            this.discordClient = discordClient;
        }

        /// <summary>
        /// The <see cref="DiscordTeam"/>'s factory.
        /// </summary>
        /// <param name="name">The team's name.</param>
        /// <returns>The newly created <see cref="DiscordTeam"/>.</returns>
        public delegate DiscordTeam Factory(string name);

        /// <summary>
        /// Gets the name of the team.
        /// </summary>
        public string Name { get; private set; } = null!;

        /// <summary>
        /// Gets the team's board channel.
        /// </summary>
        public DiscordChannel BoardChannel { get; private set; } = null!;

        public static async Task UpdateBoard(Team team, Image boardImage)
        {
            await instances[team.RowId].UpdateBoardMessage(boardImage);
        }

        public static void TeamDeleted(Team team)
        {
            if (instances.ContainsKey(team.RowId))
            {
                // TODO: JR - dispose of the instance
                instances.Remove(team.RowId);
            }
        }

        /// <summary>
        /// Creates and initializes the team's channels if they do not exist.
        /// </summary>
        /// <param name="preExisting">Weather or not the team has previously been created. (Remove with DB hookup.)</param>
        /// <param name="guild">The <see cref="DiscordClient"/> under which the team was created.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InitialiseAsync(Team? existingTeam)
        {
            changeTileButtonId = Name + "_change_tile_button";
            submitEvidenceButtonId = Name + "_submit_evidence_button";
            viewEvidenceButtonId = Name + "_view_evidence_button";

            if (existingTeam != null)
            {
                team = existingTeam;
                BoardChannel = await discordClient.GetChannelAsync(team.BoardChannelId);
                boardMessage = await BoardChannel.GetMessageAsync(team.BoardMessageId);
                //SetTeamsNoTasks();
            }
            else
            {
                await CreateChannels();
                await InitialiseBoardChannel();
                CreateTeamEntry();
                await UpdateBoardMessage(BoardImage.CreateBoard(team));
            }

            instances[team.RowId] = this;
            RegisterBoardChannelComponentInteractions();
        }

        private void CreateTeamEntry()
        {
            //team = dataWorker.Teams.Create(Name, BoardChannel.Id, boardMessage.Id);
            team = TeamRecord.CreateTeam(dataWorker, Name, BoardChannel.Id, boardMessage.Id);
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

        private async Task UpdateBoardMessage(Image boardImage)
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

            string imageName = "Team board.png";
            boardImage.SaveAsPng(imageName);
            //await boardMessage.ModifyAsync("Updating...",
            //new DiscordAttachment());



            DiscordMessage imageMessage;
            using (var fs = new FileStream(imageName, FileMode.Open, FileAccess.Read))
            {
                imageMessage = await BoardChannel.SendMessageAsync(new DiscordMessageBuilder()
                    .WithFile("Team board.png", fs));
            }

            var a = new DiscordEmbedBuilder()
            {
                Title = "Ahhhhhhh",
                ImageUrl = imageMessage.Attachments[0].Url
            }
            .Build();

            var builder = new DiscordMessageBuilder()
                .WithEmbed(a)
                .AddComponents(changeTileButton, submitEvidenceButton, viewEvidenceButton);
            //.WithFile("Team board.png", fs, true);
            await boardMessage.ModifyAsync(builder);
            await imageMessage.DeleteAsync();

        }

        private async Task InitialiseBoardChannel()
        {
            var builder = new DiscordMessageBuilder()
                .WithContent("Loading...");
            boardMessage = await BoardChannel.SendMessageAsync(builder);
        }

        private void RegisterBoardChannelComponentInteractions()
        {
            ComponentInteractionHandler.InitialisationInfo info = new ()
            {
                Team = this,
            };

            ComponentInteractionHandler.Register<ChangeTileButtonHandler>(changeTileButtonId, info);
            ComponentInteractionHandler.Register<SubmitEvidenceButtonHandler>(submitEvidenceButtonId, info);
            //ComponentInteractionHandler.Register<ViewEvidenceButtonHandler>(viewEvidenceButtonId, info);
        }
    }
}