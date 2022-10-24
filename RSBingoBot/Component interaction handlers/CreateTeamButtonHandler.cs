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
        public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info)
        {
            await base.InitialiseAsync(args, info);

            var teamNameInput = new TextInputComponent("Team name", teamNameInputId);

            var builder = new DiscordInteractionResponseBuilder()
                .WithTitle("Create a team.")
                .WithCustomId(modalId)
                .AddComponents(teamNameInput);

            await args.Interaction.CreateResponseAsync(InteractionResponseType.Modal, builder);
            SubscribeModal(new (CustomId: modalId), TeamNameSubmitted);
        }

        private async Task TeamNameSubmitted(DiscordClient discordClient, ModalSubmitEventArgs args)
        {
            string teamName = args.Values[teamNameInputId];
            var builder = new DiscordInteractionResponseBuilder()
                .AsEphemeral();

            if (Team.TeamNames.Contains(teamName))
            {
                builder.WithContent("A team with this name already exists.");
            }
            else
            {
                Team.TeamNames.Add(teamName);
                DiscordRole? role = await args.Interaction.Guild.CreateRoleAsync(teamName);
                await args.Interaction.Guild.GetMemberAsync(args.Interaction.User.Id).Result.GrantRoleAsync(role);
                builder.WithContent($"The team '{teamName}' has been created successfully.");
            }

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
        }
    }
}
