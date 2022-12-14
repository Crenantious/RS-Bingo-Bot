// <copyright file="CreateTeamButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;
    using RSBingo_Framework.Models;
    using RSBingoBot.Discord_event_handlers;

    /// <summary>
    /// Handles the interaction with the "Team sign-up" select menu in create-team channel.
    /// </summary>
    internal class CreateTeamButtonHandler : ComponentInteractionHandler
    {
        private readonly string modalId = Guid.NewGuid().ToString();
        private readonly string teamNameInputId = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets the custom Id for the "Create team" button.
        /// </summary>
        public static string CreateTeamButtonId { get; } = "create-team-button";

        /// <inheritdoc/>
        protected override bool ContinueWithNullUser { get { return true; } }

        /// <inheritdoc/>
        public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info)
        {
            await base.InitialiseAsync(args, info);

            var teamNameInput = new TextInputComponent("Team name", teamNameInputId);

            var builder = new DiscordInteractionResponseBuilder()
                .WithTitle("Create a team.")
                .WithCustomId(modalId)
                .AddComponents(teamNameInput);

            await args.Interaction.CreateResponseAsync(InteractionResponseType.Modal, builder);
            SubscribeModal(new ModalSubmittedDEH.Constraints(customId: modalId), TeamNameSubmitted);
        }

        private async Task TeamNameSubmitted(DiscordClient discordClient, ModalSubmitEventArgs args)
        {
            User = DataWorker.Users.GetByDiscordId(args.Interaction.User.Id);

            if (await UserInDBCheck(args.Interaction.User.Id, false, args) == -1) { return; }

            string teamName = args.Values[teamNameInputId];
            var builder = new DiscordFollowupMessageBuilder()
                .AsEphemeral();

            if (DataWorker.Teams.DoesTeamExist(teamName))
            {
                builder.WithContent("A team with this name already exists.");
                await args.Interaction.CreateFollowupMessageAsync(builder);
            }
            else
            {
                builder.WithContent("Creating team...");
                DiscordMessage? followupMessage = await args.Interaction.CreateFollowupMessageAsync(builder);

                InitialiseTeam team = new(discordClient, teamName);
                await team.InitialiseAsync(false, args.Interaction.Guild);

                User = DataWorker.Users.Create(args.Interaction.User.Id, teamName);
                DataWorker.SaveChanges();

                DiscordRole? role = await args.Interaction.Guild.CreateRoleAsync(teamName);
                await args.Interaction.Guild.GetMemberAsync(args.Interaction.User.Id).Result.GrantRoleAsync(role);

                string content = $"The team '{teamName}' has been created successfully.";

                if (args.Interaction.GetFollowupMessageAsync(followupMessage.Id).Result != null)
                {
                    var editBuilder = new DiscordWebhookBuilder()
                        .WithContent(content);
                    await args.Interaction.EditFollowupMessageAsync(followupMessage.Id, editBuilder);
                }
                else
                {
                    // TODO: Figure out why this doesn't do anything.
                    var newBuilder = new DiscordFollowupMessageBuilder()
                        .WithContent(content)
                        .AsEphemeral();
                    await args.Interaction.CreateFollowupMessageAsync(newBuilder);
                }
            }
            await InteractionConcluded();
        }
    }
}
