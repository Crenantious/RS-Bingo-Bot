// <copyright file="RequestAddToTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;

/// <summary>
/// Request for adding a user to a team.
/// </summary>
internal class RequestAddToTeam : RequestBase
{
    private const string UserIsAlreadyOnATeamResponse = "This user '{0}' is already on a team.";
    private const string TeamDoesNotExistResponse = "A team with the name '{0}' does not exist.";
    private const string UserSuccessfullyAddedMessage = "The user '{0}' has been added to the team successfully.";
    private const string TeamRoleDoesNotExistError = "The team's role does not exist.";

    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly string teamName;
    private readonly DiscordUser user;
    private readonly DiscordInteraction interaction;

    public RequestAddToTeam(DiscordInteraction interaction, string teamName, DiscordUser user) : base(semaphore)
    {
        this.interaction = interaction;
        this.teamName = teamName;
        this.user = user;
    }

    protected override async Task Process()
    {
        DataWorker.Users.Create(user.Id, teamName);
        DiscordRole? role = interaction.Guild.GetRole(DataWorker.Teams.GetByName(teamName).RoleId);

        if (role is null)
        {
            AddResponse(TeamRoleDoesNotExistError);
            return;
        }

        DiscordMember member = await interaction.Guild.GetMemberAsync(user.Id);
        await member.GrantRoleAsync(role);
    }

    protected override bool Validate()
    {
        if (IsUserOnATeam()) { AddResponse(UserIsAlreadyOnATeamResponse.FormatConst(user.Username)); }
        if (DataWorker.Teams.DoesTeamExist(teamName) is false) { AddResponse(TeamDoesNotExistResponse.FormatConst(teamName)); }
        return Responses.Any() is false;
    }

    private bool IsUserOnATeam() =>
        DataWorker.Users.FirstOrDefault(u => u.DiscordUserId == user.Id) is not null;
}