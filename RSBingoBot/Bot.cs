// <copyright file="Bot.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot
{
    using System.Threading;
    using DSharpPlus;
    using DSharpPlus.Interactivity;
    using DSharpPlus.Interactivity.Extensions;
    using DSharpPlus.SlashCommands;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using RSBingo_Common;
    using RSBingo_Framework.DAL;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using RSBingoBot.BingoCommands;
    using RSBingoBot.Component_interaction_handlers;
    using RSBingoBot.Discord_event_handlers;
    using RSBingoBot.Imaging;
    using RSBingoBot.Interfaces;
    using static RSBingo_Framework.DAL.DataFactory;

    /// <summary>
    /// Class for storing code related to the long running discord bot service.
    /// </summary>
    public class Bot : BackgroundService
    {
        private readonly ILogger logger;
        private readonly DiscordClient discordClient;
        private readonly InitialiseTeam.Factory teamFactory;
        private readonly ComponentInteractionDEH componentInteractionDEH;
        private readonly MessageCreatedDEH messageCreatedDEH;
        private readonly ModalSubmittedDEH modalSubmittedDEH;
        private readonly IDataWorker dataWorker = CreateDataWorker();

        /// <summary>
        /// Initializes a new instance of the <see cref="Bot"/> class.
        /// </summary>
        /// <param name="logger">The logger the instance will log to.</param>
        /// <param name="client">The client the bot will connect to.</param>
        /// <param name="teamFactory">The factory used to create instances of <see cref="Team"/>.</param>
        /// <param name="componentInteractionDEH">The DEH for component interactions.</param>
        /// <param name="messageCreatedDEH">The DEH for message creation.</param>
        /// <param name="modalSubmittedDEH">The DEH for modal submissions.</param>
        public Bot(ILogger<Bot> logger, DiscordClient client, InitialiseTeam.Factory teamFactory,
            ComponentInteractionDEH componentInteractionDEH, MessageCreatedDEH messageCreatedDEH,
            ModalSubmittedDEH modalSubmittedDEH)
        {
            this.logger = logger;
            this.discordClient = client;
            this.teamFactory = teamFactory;
            this.componentInteractionDEH = componentInteractionDEH;
            this.messageCreatedDEH = messageCreatedDEH;
            this.modalSubmittedDEH = modalSubmittedDEH;
        }

        /// <inheritdoc/>
        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            discordClient.UseInteractivity();

            SlashCommandsExtension slash = discordClient.UseSlashCommands(new SlashCommandsConfiguration()
            {
                Services = General.DI,
            });
            slash.RegisterCommands<CommandController>(Guild.Id);

            discordClient.ComponentInteractionCreated += componentInteractionDEH.OnEvent;
            discordClient.MessageCreated += messageCreatedDEH.OnEvent;
            discordClient.ModalSubmitted += modalSubmittedDEH.OnEvent;

            ComponentInteractionHandler.Register<CreateTeamButtonHandler>(CreateTeamButtonHandler.CreateTeamButtonId);
            ComponentInteractionHandler.Register<JoinTeamButtonHandler>(JoinTeamButtonHandler.JoinTeamButtonId);

            await discordClient.ConnectAsync();
            await CreateExistingTeams();
        }

        /// <inheritdoc/>
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await discordClient.DisconnectAsync();
        }

        /// <inheritdoc/>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
            => Task.CompletedTask;

        private async Task CreateExistingTeams()
        {
            foreach (Team team in dataWorker.Teams.GetTeams())
            {
                InitialiseTeam initialiseTeam = new (discordClient, team.Name);
                await initialiseTeam.InitialiseAsync(team);
            }
        }
    }
}