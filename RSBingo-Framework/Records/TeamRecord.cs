// <copyright file="TeamRecord.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Records
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RSBingo_Framework.DAL;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using RSBingo_Framework.Repository;
    using static RSBingo_Common.General;

    public static class TeamRecord
    {
        public static Team CreateTeam(IDataWorker dataWorker, string name, ulong boardChannelId, ulong boardMessageId) =>
            dataWorker.Teams.Create(name, boardChannelId, boardMessageId);

        public static bool IsBoardVerfied(this Team team) =>
            team.Tiles.Count == MaxTilesOnABoard &&
            team.Tiles.FirstOrDefault(t => t.IsNotVerified()) == null;
    }
}