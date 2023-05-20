// <copyright file="RequestRenameTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

using RSBingo_Framework.Models;
using RSBingo_Framework.Interfaces;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using static RSBingoBot.DiscordTeam;
using RSBingoBot.Leaderboard;

/// <summary>
/// Request for removing a user from a team.
/// </summary>
public class RequestRenameTeam : RequestBase
{
    private const string TeamDoesNotExistError = "No team with this name was found.";
    private const string TeamSuccessfullyRenamed = "The team has been renamed to '{0}'.";

    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly string currentName;
    private readonly string newName;
    private readonly Team? team;

    public RequestRenameTeam(InteractionContext ctx, IDataWorker dataWorker, string currentName, string newName) : base(ctx, dataWorker)
    {
        this.currentName = currentName;
        this.newName = newName;
        team = dataWorker.Teams.GetByName(currentName);
    }

    public override async Task<bool> ProcessRequest()
    {
        await semaphore.WaitAsync();

        try
        {
            await RenameTeam();

            // HACK: the dw is also saved when the request is finished. This will need to be handled properly.
            // Probably don't auto save the dw after the request.
            DataWorker.SaveChanges();
            await LeaderboardDiscord.Update(DataWorker);
        }
        catch (Exception ex) { throw; }
        finally { semaphore.Release(); }

        return ProcessSuccess(TeamSuccessfullyRenamed.FormatConst(newName));
    }

    private protected override bool ValidateSpecificRequest()
    {
        IEnumerable<string> errors;

        if (team is null) { errors = new string[] { TeamDoesNotExistError }; }
        else { errors = RequestsUtilities.GetNewTeamNameErrors(newName, DataWorker); }

        SetResponseMessage(MessageUtilities.GetCompiledMessages(errors));
        return errors.Any() is false;
    }

    private async Task RenameTeam()
    {
        team!.Name = newName;
        await RenameChannels();
        await RenameRole();
    }

    private async Task RenameChannels()
    {
        await RenameChannel(team!.CategoryChannelId, boardChannelName);
        await RenameChannel(team.BoardChannelId, boardChannelName);
        await RenameChannel(team.GeneralChannelId, boardChannelName);
        await RenameChannel(team.EvidencelChannelId, boardChannelName);
        await RenameChannel(team.VoiceChannelId, boardChannelName);
    }

    private async Task RenameChannel(ulong channelId, string nameConst)
    {
        DiscordChannel channel = Ctx.Guild.GetChannel(channelId);
        await channel.ModifyAsync((model) => model.Name = nameConst.FormatConst(newName));
    }

    private async Task RenameRole()
    {
        await Ctx.Guild.GetRole(team!.RoleId)
            .ModifyAsync(RSBingoBot.DiscordTeam.RoleName.FormatConst(newName));
    }
}