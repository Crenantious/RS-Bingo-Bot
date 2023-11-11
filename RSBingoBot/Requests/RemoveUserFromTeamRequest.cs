// <copyright file="RemoveUserFromTeamRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;
using DSharpPlus.Entities;

/// <summary>
/// Request for removing a user from a team.
/// </summary>
internal class RemoveUserFromTeamRequest : RequestHandlerBase
{
    private const string UserIsNotOnATeamResponse = "The user '{0}' is not on a team.";
    private const string UserIsNotOnTheTeam = "The user '{0}' is not on that team; they are on the team '{0}'.";
    private const string TeamDoesNotExistError = "A team with the name '{0}' does not exist.";
    private const string UserSuccessfullyRemovedMessage = "The user '{0}' has been removed from the team.";
    private const string TeamRoleDoesNotExistError = "The team's role does not exist.";

    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly string teamName;
    private readonly DiscordUser user;
    private readonly User? databaseUser;
    private readonly DiscordInteraction interaction;

    public RemoveUserFromTeamRequest(DiscordInteraction interaction, string teamName, DiscordUser user) : base(semaphore)
    {
        this.interaction = interaction;
        this.teamName = teamName;
        this.user = user;
        databaseUser = DataWorker.Users.FirstOrDefault(u => u.DiscordUserId == user.Id);
    }

    protected override bool Validate()
    {
        bool checkIfUserIsOnTheTeam = true;

        if (IsUserOnATeam() is false)
        {
            checkIfUserIsOnTheTeam = false;
            AddResponse(UserIsNotOnATeamResponse.FormatConst(user.Username));
        }

        if (DataWorker.Teams.DoesTeamExist(teamName) is false)
        {
            checkIfUserIsOnTheTeam = false;
            AddResponse(TeamDoesNotExistError.FormatConst(teamName));
        }

        else if (checkIfUserIsOnTheTeam && IsUserOnTheTeam() is false)
        {
            AddResponse(UserIsNotOnTheTeam.FormatConst(user.Username, teamName));
        }

        return Responses.Any() is false;
    }

    protected async override Task Process()
    {
        DataWorker.Users.Remove(databaseUser!);
        AddResponse(UserSuccessfullyRemovedMessage.FormatConst(user.Username));

        DiscordRole? role = interaction.Guild.GetRole(databaseUser!.Team.RoleId);
        if (role is null)
        {
            AddResponse(TeamRoleDoesNotExistError);
            return;
        }

        DiscordMember member = await interaction.Guild.GetMemberAsync(user.Id);
        await member.RevokeRoleAsync(role);
    }

    private bool IsUserOnATeam() =>
        databaseUser is not null;

    private bool IsUserOnTheTeam() =>
        databaseUser!.Team.Name == teamName;
}