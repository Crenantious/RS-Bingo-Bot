// <copyright file="TeamRecord.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Records
{
    using System.Collections.Generic;
    using System.Linq;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using RSBingo_Framework.Repository;

    public static class TeamRecord
    {
        public static Team CreateTeam(IDataWorker dataWorker, string name, ulong boardChannelId) =>
            dataWorker.Teams.Create(name, boardChannelId);

        public static bool IsBoardVerfied(this Team team) =>
            team.Tiles.FirstOrDefault(t => t.IsNotVerified()) == null;

        public static IEnumerable<Tile> GetNoTaskTiles(this Team team) =>
            team.Tiles.Where(t => t.Task.Name == BingoTaskRepository.NoTaskName);

        public static IEnumerable<Tile> GetNonNoTaskTiles(this Team team) =>
            team.Tiles.Where(t => t.Task.Name != BingoTaskRepository.NoTaskName);
    }
}
