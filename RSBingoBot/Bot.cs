// <copyright file="Bot.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BingoBotEmbed
{
    using System.Threading;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.EventArgs;
    using DSharpPlus.Interactivity;
    using DSharpPlus.Interactivity.Extensions;
    using DSharpPlus.SlashCommands;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Class for storing code related to the long running discord bot service.
    /// </summary>
    public class Bot : BackgroundService
    {
        private ILogger logger;
        private DiscordClient discordClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bot"/> class.
        /// </summary>
        /// <param name="logger">The logger the instance will log to.</param>
        /// <param name="client">The client the bot will connect to.</param>
        public Bot(ILogger<Bot> logger, DiscordClient client)
        {
            this.logger = logger;
            this.discordClient = client;
        }

        /// <inheritdoc/>
        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            discordClient.MessageCreated += DiscordClient_MessageCreated;
            await discordClient.ConnectAsync();
        }

        /// <inheritdoc/>
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await discordClient.DisconnectAsync();
        }

        /// <inheritdoc/>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
            => Task.CompletedTask;

        private Task DiscordClient_MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}