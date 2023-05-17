// <copyright file="RequestAddToTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

using RSBingo_Framework.Interfaces;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

/// <summary>
/// Request for adding a user to a team.
/// </summary>
public class RequestAddToTeam : RequestBase
{
    private const string UserIsAlreadyOnATeam = "This user is already on a team.";
    private const string TeamDoesNotExistsMessage = "No team with this name was found.";
    private const string UserSuccessfullyAddedMessage = "The user '{0}' has been added to the team successfully.";

    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly string teamName;
    private readonly DiscordUser user;

    public RequestAddToTeam(InteractionContext ctx, IDataWorker dataWorker, string teamName, DiscordUser user) : base(ctx, dataWorker)
    {
        this.teamName = teamName;
        this.user = user;
    }

    public override async Task<bool> ProcessRequest()
    {
        await semaphore.WaitAsync();
        await AddUser();
        semaphore.Release();
        return ProcessSuccess(UserSuccessfullyAddedMessage.FormatConst(user.Username));
    }

    private protected override bool ValidateSpecificRequest()
    {
        List<string> errors = new(2);

        if (IsUserOnATeam()) { errors.Add(UserIsAlreadyOnATeam); }
        if (DataWorker.Teams.DoesTeamExist(teamName) is false) { errors.Add(TeamDoesNotExistsMessage); }

        SetResponseMessage(MessageUtilities.GetCompiledMessages(errors));

        return errors.Any() is false;
    }

    private bool IsUserOnATeam() =>
        DataWorker.Users.FirstOrDefault(u => u.DiscordUserId == user.Id) is not null;

    private async Task AddUser()
    {
        DataWorker.Users.Create(user.Id, teamName);
        DiscordMember member = await Ctx.Guild.GetMemberAsync(user.Id);
        DiscordRole role = Ctx.Guild.GetRole(DataWorker.Teams.GetByName(teamName).RoleId);
        await member.GrantRoleAsync(role);
    }
}