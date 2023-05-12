// <copyright file="JoinTeamButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers;

using RSBingoBot.Discord_event_handlers;
using RSBingo_Framework.Models;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using static RSBingoBot.MessageUtilities;

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

        if (await ValidateUser()) { await TeamSelectionResponse(); }
    }

    private async Task<bool> ValidateUser()
    {
        if (DataWorker.Users.Exists(OriginalInteractionArgs.Interaction.User.Id))
        {
            await Respond(OriginalInteractionArgs, AlreadyOnATeamMessage, true);
            await ConcludeInteraction();
            return false;
        }
        return true;
    }

    private async Task TeamSelectionResponse()
    {
        if (await TrySendUserTeamStatusErrorMessage(OriginalInteractionArgs.User.Id, false, OriginalInteractionArgs))
        {
            await ConcludeInteraction();
            return;
        }

        IEnumerable<Team> teams = DataWorker.Teams.GetTeams();

        if (teams.Any() is false)
        {
            await Respond(OriginalInteractionArgs, "No teams created.", true);
            await ConcludeInteraction();
            return;
        }

        IEnumerable<DiscordSelectComponentOption> options = teams.Select(t => new DiscordSelectComponentOption(t.Name, t.Name));

        DiscordSelectComponent teamSelect = new(teamSelectId, "Select team", options);
        SubscribeComponent(
            new ComponentInteractionDEH.Constraints(user: OriginalInteractionArgs.User, channel: OriginalInteractionArgs.Channel, customId: teamSelectId),
            TeamSelected, true);

        DiscordButtonComponent confirmButton = new(ButtonStyle.Primary, confirmButtonId, "Confirm");
        SubscribeComponent(
            new ComponentInteractionDEH.Constraints(user: OriginalInteractionArgs.User, channel: OriginalInteractionArgs.Channel, customId: confirmButtonId),
            TeamJoinConfirmed, true);

        var builder = new DiscordInteractionResponseBuilder()
            .WithContent($"{OriginalInteractionArgs.User.Mention} Select a team to join.")
            .AddComponents(teamSelect)
            .AddComponents(confirmButton);

        await OriginalInteractionArgs.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
        MessagesForCleanup.Add(await OriginalInteractionArgs.Interaction.GetOriginalResponseAsync());
    }

    private async Task TeamSelected(DiscordClient discordClient, ComponentInteractionCreateEventArgs args) =>
        teamSelected = args.Values[0];

    private async Task TeamJoinConfirmed(DiscordClient discordClient, ComponentInteractionCreateEventArgs args)
    {
        if (teamSelected == string.Empty)
        {
            await Followup(args, "You must select a team to join.", true);
            return;
        }

        await JoinTeam(args);
    }

    private async Task JoinTeam(ComponentInteractionCreateEventArgs args)
    {
        string content = string.Empty;

        Team? team = DataWorker.Teams.GetByName(teamSelected);

        DataWorker.Users.Create(args.User.Id, team);
        DataWorker.SaveChanges();

        content = $"You have joined team {teamSelected}.";

        DiscordRole role = RSBingoBot.DiscordTeam.GetInstance(team).Role;

        try
        {
            await args.Guild.GetMemberAsync(args.User.Id).Result.GrantRoleAsync(role);
        }
        catch (DSharpPlus.Exceptions.NotFoundException)
        {
            content += $"{Environment.NewLine}The team's role does not exist; please tell a host.";
        }

        await Followup(args, content, true);
        await ConcludeInteraction();
    }
}