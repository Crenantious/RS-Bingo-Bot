// <copyright file="JoinTeamButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;
    using RSBingoBot.Discord_event_handlers;

    /// <summary>
    /// Handles the interaction with the "Team sign-up" select menu in create-team channel.
    /// </summary>
    internal class JoinTeamButtonHandler : ComponentInteractionHandler
    {
        private readonly string confirmButtonId = Guid.NewGuid().ToString();
        private readonly string teamSelectId = Guid.NewGuid().ToString();
        private string teamSelected = string.Empty;

        /// <summary>
        /// Gets the custom Id for the "Join team" button.
        /// </summary>
        public static string JoinTeamButtonId { get; } = "join-team-button";

        /// <inheritdoc/>
        public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info)
        {
            await base.InitialiseAsync(args, info);

            // If the user is already in a team, give them an error.
            // They must be removed from the team by an admin before being able to join another.

            var confirmButton = new DiscordButtonComponent(ButtonStyle.Primary, confirmButtonId, "Confirm");
            SubscribeComponent(new ComponentInteractionDEH.Constraints(user: args.User, channel: args.Channel, customId: confirmButtonId), TeamJoinConfirmed);

            var builder = new DiscordInteractionResponseBuilder();

            if (InitialiseTeam.TeamNames.Count == 0)
            {
                builder
                   .WithContent("No teams created.")
                   .AsEphemeral();
            }
            else
            {
                var options = new List<DiscordSelectComponentOption>();
                foreach (string team in InitialiseTeam.TeamNames)
                {
                    options.Add(new (team, team));
                }

                var teamSelect = new DiscordSelectComponent(teamSelectId, "Select team", options);
                SubscribeComponent(new ComponentInteractionDEH.Constraints(user: args.User, channel: args.Channel, customId: teamSelectId), TeamSelected);

                builder
                    .WithContent($"{args.User.Mention} Select a team to join.")
                    .AddComponents(teamSelect)
                    .AddComponents(confirmButton);
            }

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
        }

        private async Task TeamSelected(DiscordClient discordClient, ComponentInteractionCreateEventArgs args)
        {
            teamSelected = args.Values[0];
            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
        }

        private async Task TeamJoinConfirmed(DiscordClient discordClient, ComponentInteractionCreateEventArgs args)
        {
            string content = string.Empty;

            if (teamSelected == string.Empty)
            {
                content = "You must select a team to join.";
            }
            else
            {
                var roles = args.Guild.Roles.Select(x => x.Value).ToList();
                int index = roles.Select(x => x.Name).ToList().IndexOf(teamSelected);

                content = $"You have joined team {teamSelected}.";

                if (index == -1)
                {
                    // Error, team role should exist
                    content += "\nThe team's role does not exist, please tell an admin.";
                }
                else
                {
                    await args.Guild.GetMemberAsync(args.User.Id).Result.GrantRoleAsync(roles[index]);
                }

                await OriginalInteractionArgs.Interaction.GetOriginalResponseAsync().Result.DeleteAsync();
            }

            var builder = new DiscordInteractionResponseBuilder()
                .WithContent(content)
                .AsEphemeral();

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
        }
    }
}
