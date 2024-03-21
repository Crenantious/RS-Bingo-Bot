// <copyright file="TeamRecord.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Records;

using RSBingo_Common;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using static RSBingo_Common.General;

public static class TeamRecord
{
    public static Team CreateTeam(IDataWorker dataWorker, string name, ulong categoryChannelId,
        ulong boardChannelId, ulong generalChannelId, ulong evidenceChannelId, ulong voiceChannelId, ulong boardMessageId, ulong roleId) =>
        dataWorker.Teams.Create(name, categoryChannelId, boardChannelId, generalChannelId, evidenceChannelId, voiceChannelId, boardMessageId, roleId);

    public static bool IsBoardVerfied(this Team team) =>
        team.Tiles.Count == MaxTilesOnABoard &&
        team.Tiles.FirstOrDefault(t => t.IsVerified() is false) == null;

    /// <summary>
    /// Creates tiles for the team equal to Max(<see cref="General.MaxTilesOnABoard"/>, tasks.Count()). The tasks are ordered by their row id.
    /// </summary>
    public static void CreateDefaultTiles(this Team team, IDataWorker dataWorker)
    {
        IEnumerable<BingoTask> tasks = dataWorker.BingoTasks.GetAll()
            .Take(General.MaxTilesOnABoard)
            .OrderBy(t => t.RowId);

        for (int i = 0; i < tasks.Count(); i++)
        {
            dataWorker.Tiles.Create(team, tasks.ElementAt(i), i);
        }
    }
}