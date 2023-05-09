// <copyright file="TeamRecord.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Records;

using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using static RSBingo_Common.General;

public static class TeamRecord
{
    public static Team CreateTeam(IDataWorker dataWorker, string name, ulong categoryChannelId,
        ulong boardChannelId, ulong generalChannelId, ulong voiceChannelId, ulong boardMessageId, ulong roleId) =>
        dataWorker.Teams.Create(name, categoryChannelId, boardChannelId, generalChannelId, voiceChannelId, boardMessageId, roleId);

    public static bool IsBoardVerfied(this Team team) =>
        team.Tiles.Count == MaxTilesOnABoard &&
        team.Tiles.FirstOrDefault(t => t.IsNotVerified()) == null;
}