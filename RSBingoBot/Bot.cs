﻿// <copyright file="Bot.cs" company="PlaceholderCompany">
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
    using RSBingoBot.BingoCommands;
    using RSBingoBot.Discord_event_handlers;

    /// <summary>
    /// Class for storing code related to the long running discord bot service.
    /// </summary>
    public class Bot : BackgroundService
    {
        private readonly ILogger logger;
        private readonly DiscordClient discordClient;
        private readonly Team.Factory teamFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bot"/> class.
        /// </summary>
        /// <param name="logger">The logger the instance will log to.</param>
        /// <param name="client">The client the bot will connect to.</param>
        /// <param name="teamFactory">The factory used to create instances of <see cref="Team"/>.</param>
        public Bot(ILogger<Bot> logger, DiscordClient client, Team.Factory teamFactory)
        {
            this.logger = logger;
            this.discordClient = client;
            this.teamFactory = teamFactory;
        }

        /// <inheritdoc/>
        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            discordClient.UseInteractivity();

            SlashCommandsExtension slash = discordClient.UseSlashCommands(new SlashCommandsConfiguration()
            {
                Services = General.DI,
            });
            slash.RegisterCommands<CommandController>(ulong.Parse(DataFactory.TestGuildId));

            discordClient.ComponentInteractionCreated += ComponentInteractionDEH.OnEvent;
            discordClient.MessageCreated += MessageCreatedDEH.OnEvent;

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
            // Placeholder. Will create all teams existing in DB
            Team team = teamFactory("Test");
            await team.InitialiseAsync(true);
        }
    }
}