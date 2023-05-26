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
using RSBingoBot.Imaging;

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
        else { errors = RequestsUtilities.ValidateNewTeamName(newName, DataWorker); }

        SetResponseMessage(MessageUtilities.GetCompiledMessages(errors));
        return errors.Any() is false;
    }

    private async Task RenameTeam()
    {
        BoardImage.RenameTeam(team!.Name, newName);
        team.Name = newName;
        await RSBingoBot.DiscordTeam.GetInstance(team).Rename(newName);
    }
}