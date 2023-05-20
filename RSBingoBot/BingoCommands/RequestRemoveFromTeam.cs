// <copyright file="RequestRemoveFromTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

using RSBingo_Framework.Interfaces;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RSBingo_Framework.Models;

/// <summary>
/// Request for removing a user from a team.
/// </summary>
public class RequestRemoveFromTeam : RequestBase
{
    private const string UserIsNotOnATeam = "This user is not on a team.";
    private const string UserIsNotOnTheTeam = "This user is not on that team. They are on the team '{0}'.";
    private const string TeamDoesNotExistError = "No team with this name was found.";
    private const string UserSuccessfullyRemovedMessage = "The user '{0}' has been removed from the team.";
    private const string TeamRoleDoesNotExistError = "The team's role does not exist.";

    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly string teamName;
    private readonly DiscordUser user;
    private readonly User? databaseUser;

    public RequestRemoveFromTeam(InteractionContext ctx, IDataWorker dataWorker, string teamName, DiscordUser user) : base(ctx, dataWorker)
    {
        this.teamName = teamName;
        this.user = user;
        databaseUser = DataWorker.Users.FirstOrDefault(u => u.DiscordUserId == user.Id);
    }

    public override async Task<bool> ProcessRequest()
    {
        await semaphore.WaitAsync();

        try
        {
            if (await RemoveUser() is string error) { return ProcessFailure(error); }
        }
        catch (Exception ex) { throw; }
        finally { semaphore.Release(); }

        return ProcessSuccess(UserSuccessfullyRemovedMessage.FormatConst(user.Username));
    }

    private protected override bool ValidateSpecificRequest()
    {
        List<string> errors = new(3);
        bool checkIfUserIsOnTheTeam = true;

        if (IsUserOnATeam() is false)
        {
            checkIfUserIsOnTheTeam = false;
            errors.Add(UserIsNotOnATeam);
        }

        if (DataWorker.Teams.DoesTeamExist(teamName) is false)
        {
            checkIfUserIsOnTheTeam = false;
            errors.Add(TeamDoesNotExistError);
        }

        else if (checkIfUserIsOnTheTeam && IsUserOnTheTeam() is false)
        {
            errors.Add(UserIsNotOnTheTeam.FormatConst(databaseUser.Team.Name));
        }

        SetResponseMessage(MessageUtilities.GetCompiledMessages(errors));

        return errors.Any() is false;
    }

    private bool IsUserOnATeam() =>
        databaseUser is not null;

    private bool IsUserOnTheTeam() =>
        IsUserOnATeam() &&
        databaseUser!.Team.Name == teamName;

    private async Task<string?> RemoveUser()
    {
        DiscordMember member = await Ctx.Guild.GetMemberAsync(user.Id);
        DiscordRole? role = Ctx.Guild.GetRole(DataWorker.Teams.GetByName(teamName).RoleId);

        if (role is null) { return TeamRoleDoesNotExistError; }

        await member.RevokeRoleAsync(role);
        DataWorker.Users.Remove(databaseUser!);
        return null;
    }
}