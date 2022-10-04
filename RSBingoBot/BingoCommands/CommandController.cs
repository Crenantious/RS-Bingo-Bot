﻿// <copyright file="CommandController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands
{
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.SlashCommands;
    using Microsoft.Extensions.Logging;
    using RSBingoBot;

    /// <summary>
    /// Controller class for discoed bot commands.
    /// </summary>
    public class CommandController : ApplicationCommandModule
    {
        private const string TestTeamName = "Test";

        private readonly ILogger<CommandController> logger;
        private readonly DiscordClient discordClient;
        private readonly Team.Factory teamFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandController"/> class.
        /// </summary>
        /// <param name="logger">The logger the instance will log to.</param>
        /// <param name="discordClient">The client the bot will connect to.</param>
        /// <param name="teamFactory">The factory used to create a new <see cref="Team"/> object.</param>
        public CommandController(ILogger<CommandController> logger, DiscordClient discordClient, Team.Factory teamFactory)
        {
            this.logger = logger;
            this.discordClient = discordClient;
            this.teamFactory = teamFactory;
        }

        public async Task Start(InteractionContext context)
        {
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource);

            // TOOD: JCH - Work.

            await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Content"));
        }

        /// <summary>
        /// Create and initialize a new team.
        /// </summary>
        /// <param name="ctx">The context under which the command was executed.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [SlashCommand("CreateTestTeamChannels", $"Creates a new team named {TestTeamName}.")]
        public async Task CreateTeam(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                .WithContent($"Creating a new team named {TestTeamName}.")
                .AsEphemeral());

            Team team = teamFactory("Test");
            await team.InitialiseAsync(false, ctx.Guild);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"New team named {TestTeamName} has been created."));
        }

        /// <summary>
        /// Deletes all channels starting with the <see cref="TestTeamName"/>.
        /// </summary>
        /// <param name="ctx">The context under which the command was executed.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [SlashCommand("DeleteTestTeamChannels", $"Deletes the team channels for the team named {TestTeamName}.")]
        public async Task DeleteTeamChannels(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                .WithContent($"Channels are being deleted for team {TestTeamName}.")
                .AsEphemeral());

            foreach (var channel in ctx.Guild.Channels)
            {
                if (channel.Value.Name.ToLower().StartsWith(TestTeamName.ToLower()))
                {
                    await channel.Value.DeleteAsync();
                }
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"Channels have been deleted for team {TestTeamName}."));
        }
    }
}