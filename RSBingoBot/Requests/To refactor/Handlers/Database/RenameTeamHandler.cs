// <copyright file="RenameTeamHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;
using RSBingoBot.Imaging;
using System.Threading;

internal class RenameTeamHandler : RequestHandler<RenameTeamRequest>
{
    private const string TeamSuccessfullyRenamed = "The team '{0}' has been renamed to '{1}'.";

    private static readonly SemaphoreSlim semaphore = new(1, 1);

    public RenameTeamHandler() : base(semaphore)
    {

    }

    protected async override Task Process(RenameTeamRequest request, CancellationToken cancellationToken)
    {
        string oldName = request.TeamName;
        string newName = request.NewTeamName;

        Team team = DataWorker.Teams.GetByName(oldName)!;

        BoardImage.RenameTeam(oldName, newName);
        team.Name = newName;
        await RSBingoBot.DiscordTeamOld.GetInstance(team).Rename(newName);

        AddSuccess(TeamSuccessfullyRenamed.FormatConst(oldName, newName));
    }
}