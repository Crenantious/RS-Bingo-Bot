// <copyright file="CommandController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.BingoCommands
{
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.SlashCommands;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Controller class for discoed bot commands.
    /// </summary>
    public class CommandController : ApplicationCommandModule
    {
        private readonly ILogger<CommandController> logger;
        private readonly DiscordClient discordClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandController"/> class.
        /// </summary>
        /// <param name="logger">The logger the instance will log to.</param>
        /// <param name="client">The client the bot will connect to.</param>
        public CommandController(ILogger<CommandController> logger, DiscordClient client)
        {
            this.logger = logger;
            discordClient = client;
        }

        public async Task Start(InteractionContext context)
        {
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource);

            // TOOD: JCH - Work.

            await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Content"));
        }
    }
}
