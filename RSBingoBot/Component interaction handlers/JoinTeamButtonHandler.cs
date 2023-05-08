// <copyright file="JoinTeamButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers;

using RSBingoBot.Discord_event_handlers;
using RSBingo_Framework.Models;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using static RSBingoBot.InteractionMessageUtilities;

/// <summary>
/// Handles the interaction with the "Join team" button.
/// </summary>
internal class JoinTeamButtonHandler : ComponentInteractionHandler
{
    private readonly string confirmButtonId = Guid.NewGuid().ToString();
    private readonly string teamSelectId = Guid.NewGuid().ToString();
    private string teamSelected = string.Empty;

    /// <inheritdoc/>
    protected override bool ContinueWithNullUser { get { return true; } }
    protected override bool CreateAutoResponse { get { return false; } }

    /// <summary>
    /// Gets the custom Id for the "Join team" button.
    /// </summary>
    public static string JoinTeamButtonId { get; } = "join-team-button";

    /// <inheritdoc/>
    public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info)
    {
        await base.InitialiseAsync(args, info);

        if (DataWorker.Users.Exists(args.Interaction.User.Id))
        {
            await Respond(args, AlreadyOnATeamMessage, true);
            await ConcludeInteraction();
        }
        else { await TeamSelectionResponse(); }
    }

    private async Task TeamSelectionResponse()
    {
        var confirmButton = new DiscordButtonComponent(ButtonStyle.Primary, confirmButtonId, "Confirm");
        SubscribeComponent(
            new ComponentInteractionDEH.Constraints(user: OriginalInteractionArgs.User, channel: OriginalInteractionArgs.Channel, customId: confirmButtonId),
            TeamJoinConfirmed, true);

        var builder = new DiscordInteractionResponseBuilder();
        IEnumerable<Team> teams = DataWorker.Teams.GetTeams();

        if (await TrySendUserTeamStatusErrorMessage(OriginalInteractionArgs.User.Id, false, OriginalInteractionArgs) is false)
        {
            await ConcludeInteraction();
        }

        if (!teams.Any())
        {
            builder
               .WithContent("No teams created.")
               .AsEphemeral();
        }
        else
        {
            var options = new List<DiscordSelectComponentOption>();
            foreach (Team team in teams)
            {
                options.Add(new(team.Name, team.Name));
            }

            var teamSelect = new DiscordSelectComponent(teamSelectId, "Select team", options);
            SubscribeComponent(
                new ComponentInteractionDEH.Constraints(user: OriginalInteractionArgs.User, channel: OriginalInteractionArgs.Channel, customId: teamSelectId),
                TeamSelected, true);

            builder
                .WithContent($"{OriginalInteractionArgs.User.Mention} Select a team to join.")
                .AddComponents(teamSelect)
                .AddComponents(confirmButton);
        }

        await OriginalInteractionArgs.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
        MessagesForCleanup.Add(await OriginalInteractionArgs.Interaction.GetOriginalResponseAsync());
    }

    private async Task TeamSelected(DiscordClient discordClient, ComponentInteractionCreateEventArgs args) =>
        teamSelected = args.Values[0];

    private async Task TeamJoinConfirmed(DiscordClient discordClient, ComponentInteractionCreateEventArgs args)
    {
        string content = string.Empty;

        if (teamSelected == string.Empty)
        {
            content = "You must select a team to join.";
        }
        else
        {
            DataWorker.Users.Create(args.User.Id, teamSelected);
            DataWorker.SaveChanges();

            var roles = args.Guild.Roles.Select(x => x.Value).ToList();
            int index = roles.Select(x => x.Name).ToList().IndexOf(teamSelected);

            content = $"You have joined team {teamSelected}.";

            if (index == -1)
            {
                // Error, team role should exist
                content += $"{Environment.NewLine}The team's role does not exist; please tell an admin.";
            }
            else
            {
                await args.Guild.GetMemberAsync(args.User.Id).Result.GrantRoleAsync(roles[index]);
            }

            await ConcludeInteraction();
        }

        var builder = new DiscordFollowupMessageBuilder()
            .WithContent(content)
            .AsEphemeral();

        await args.Interaction.CreateFollowupMessageAsync(builder);
    }
}
