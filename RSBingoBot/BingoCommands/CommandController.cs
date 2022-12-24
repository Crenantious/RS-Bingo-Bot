// <copyright file="CommandController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands
{
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;
    using DSharpPlus.SlashCommands;
    using DSharpPlus.SlashCommands.Attributes;
    using Microsoft.Extensions.Logging;
    using RSBingo_Framework;
    using RSBingo_Framework.DAL;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using RSBingoBot;
    using RSBingoBot.Component_interaction_handlers;
    using RSBingoBot.Discord_event_handlers;
    using static RSBingo_Framework.DAL.DataFactory;

    /// <summary>
    /// Controller class for discoed bot commands.
    /// </summary>
    public class CommandController : ApplicationCommandModule
    {
        private const string TestTeamName = "Test";

        private readonly ILogger<CommandController> logger;
        private readonly IDataWorker dataWorker = CreateDataWorker();
        private readonly DiscordClient discordClient;
        private readonly RSBingoBot.DiscordTeam.Factory teamFactory;
        private readonly MessageCreatedDEH messageCreatedDEH;
        private readonly ModalSubmittedDEH modalSubmittedDEH;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandController"/> class.
        /// </summary>
        /// <param name="logger">The logger the instance will log to.</param>
        /// <param name="discordClient">The client the bot will connect to.</param>
        /// <param name="teamFactory">The factory used to create a new <see cref="DiscordTeam"/> object.</param>
        public CommandController(ILogger<CommandController> logger, DiscordClient discordClient,
            RSBingoBot.DiscordTeam.Factory teamFactory, MessageCreatedDEH messageCreatedDEH,
            ModalSubmittedDEH modalSubmittedDEH)
        {
            this.logger = logger;
            this.discordClient = discordClient;
            this.teamFactory = teamFactory;
            this.messageCreatedDEH = messageCreatedDEH;
            this.modalSubmittedDEH = modalSubmittedDEH;
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
        public async Task CreateTestTeamChannels(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                .WithContent($"Creating a new team named {TestTeamName}.")
                .AsEphemeral());

            RSBingoBot.DiscordTeam team = teamFactory("Test");
            await team.InitialiseAsync(null);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"New team named {TestTeamName} has been created."));
        }

        /// <summary>
        /// Deletes all channels starting with the <see cref="TestTeamName"/>.
        /// </summary>
        /// <param name="ctx">The context under which the command was executed.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [SlashCommand("DeleteTestTeamChannels", $"Deletes the team channels for the team named {TestTeamName}.")]
        public async Task DeleteTestTeamChannels(InteractionContext ctx)
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

        /// <summary>
        /// Posts a message in the channel the command was run in with buttons to create and join a team.
        /// </summary>
        /// <param name="ctx">The context under which the command was executed.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [SlashCommand("InitializeCreateTeamChannel", $"Posts a message in the current channel with buttons to create and join a team.")]
        public async Task InitializeCreateTeamChannel(InteractionContext ctx)
        {
            var createTeamButton = new DiscordButtonComponent(ButtonStyle.Primary, CreateTeamButtonHandler.CreateTeamButtonId, "Create team");
            var joinTeamButton = new DiscordButtonComponent(ButtonStyle.Primary, JoinTeamButtonHandler.JoinTeamButtonId, "Join team");

            var builder = new DiscordMessageBuilder()
                .WithContent("Create a new team or join an existing one.")
                .AddComponents(createTeamButton, joinTeamButton);

            await ctx.Channel.SendMessageAsync(builder);

            ComponentInteractionHandler.Register<CreateTeamButtonHandler>(CreateTeamButtonHandler.CreateTeamButtonId);
            ComponentInteractionHandler.Register<JoinTeamButtonHandler>(JoinTeamButtonHandler.JoinTeamButtonId);
        }

        [SlashCommand("DeleteTeam", $"Deletes a team from the database, it's role; users; and channels.")]
        public async Task DeleteTeam(InteractionContext ctx, [Option("Name", "Team name")] string teamName)
        {
            if (!HasAdminPermission(ctx))
            {
                await InsufficientPermissionsResponse(ctx);
                return;
            }

            var builder = new DiscordInteractionResponseBuilder()
                .WithContent("Deleting team...")
                .AsEphemeral();

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);

            // Delete role
            DiscordRole? role = GetTeamRole(ctx, teamName);
            if (role != null)
            {
                // If the command is ran multiple times in quick succession,
                // it's possible for one to delete the role while another is trying
                try
                {
                    await role.DeleteAsync();
                }
                catch { }
            }

            // Delete channels
            foreach (var channelPair in ctx.Guild.Channels.Where(c => c.Value.Name.StartsWith(teamName)))
            {
                // If the command is ran multiple times in quick succession,
                // it's possible for one to delete some channels while another is trying
                try
                {
                    await channelPair.Value.DeleteAsync();
                }
                catch { }
            }

            // Delete from database
            Team? team = dataWorker.Teams.GetByName(teamName);
            string response;

            if (team == null)
            {
                response = "No team with that name exists.";
            }
            else
            {
                RSBingoBot.DiscordTeam.TeamDeleted(team);
                dataWorker.Teams.Delete(team);
                dataWorker.SaveChanges();
                response = "Team deleted.";
            }

            EditOriginalResponse(ctx, new DiscordWebhookBuilder()
                .WithContent(response));
        }

        [SlashCommand("RemoveFromTeam", $"Removes a user from a team in the database, and removes the team's role from them.")]
        public async Task RemoveFromTeam(InteractionContext ctx,
            [Option("User", "User")] DiscordUser discordUser)
        {
            User? user = dataWorker.Users.GetByDiscordId(discordUser.Id);

            var builder = new DiscordInteractionResponseBuilder()
                .AsEphemeral();

            if (user != null)
            {
                DiscordRole? role = GetTeamRole(ctx, user.Team.Name);
                if (role != null)
                {
                    await ctx.Guild.GetMemberAsync(ctx.User.Id).Result.RevokeRoleAsync(role);
                }

                dataWorker.Users.Delete(user);
                dataWorker.SaveChanges();

                builder.WithContent("User has been successfully removed from the team.");
            }
            else
            {
                builder.WithContent("This user is not on a team.");
            }

            await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
        }

        // TODO: JR - allow only in given channels
        [SlashCommand("AddTasks", $"Adds tasks to the database based on the uploaded csv file.")]
                                  //$"The csv must have the format: name, difficulty, number of tiles, restriction name.")]
        public async Task AddTasks(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
        {
            await AddDeleteTasks(ctx, attachment, true);
        }

        [SlashCommand("DeleteTasks", $"Deletes tasks from the database based on the uploaded csv file.")]
        public async Task DeleteTasks(InteractionContext ctx, [Option("Attachment", "Attachment")] DiscordAttachment attachment)
        {
            await AddDeleteTasks(ctx, attachment, false);
        }

        private async Task AddDeleteTasks(InteractionContext ctx, DiscordAttachment attachment, bool addTasks)
        {
            if (!HasAdminPermission(ctx))
            {
                await InsufficientPermissionsResponse(ctx);
                return;
            }

            var builder = new DiscordInteractionResponseBuilder()
                .WithContent("Processing... this may take a while.");
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);

            var editBuilder = new DiscordWebhookBuilder();

            if (attachment.FileName.EndsWith(".csv"))
            {
                string action = addTasks ? "add" : "delete";
                string actionVerb = addTasks ? "added" : "deleted";
                string errorMessage = addTasks ?
                    CSVReader.CreateTasks(attachment.Url) :
                    CSVReader.DeleteTasks(attachment.Url);

                if (errorMessage == string.Empty)
                {
                    editBuilder.WithContent($"Tasks successfully {actionVerb}.");
                }
                else
                {
                    editBuilder.WithContent($"The following error occurred while attempting to {action} tasks: {errorMessage}");
                }
            }
            else
            {
                editBuilder.WithContent("Incorrect file type; must be csv.");
            }

            await ctx.EditResponseAsync(editBuilder);
        }

        private async Task InsufficientPermissionsResponse(InteractionContext ctx)
        {
            // TODO: JR - Change this to a pre-execution check
            var builder = new DiscordInteractionResponseBuilder()
                .WithContent("You do not have the required permissions to run this command.")
                .AsEphemeral();
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
        }

        private bool HasAdminPermission(InteractionContext ctx) =>
            ctx.Guild.GetMemberAsync(ctx.User.Id).Result.Permissions.HasPermission(Permissions.Administrator);

        private DiscordRole? GetTeamRole(InteractionContext ctx, string teamName)
        {
            KeyValuePair<ulong, DiscordRole> pair = ctx.Interaction.Guild.Roles.FirstOrDefault(r => r.Value.Name == teamName);
            if (pair.Equals(default)) { return null; }
            return pair.Value;
        }

        private async void EditOriginalResponse(InteractionContext ctx, DiscordWebhookBuilder builder)
        {
            try
            {
                await ctx.EditResponseAsync(builder);
            }
            catch { }
        }
    }
}