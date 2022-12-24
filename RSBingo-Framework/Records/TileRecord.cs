// <copyright file="TileRecord.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Records
{
    using RSBingo_Common;
    using RSBingo_Framework.DAL;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using RSBingo_Framework.Repository;

    public static class TileRecord
    {
        #region enums & lookups

        private static readonly EnumDict<VerifiedStatus> VerifiedLookup = new EnumDict<VerifiedStatus>(VerifiedStatus.No)
         .Add(VerifiedStatus.Yes, 1);

        public enum VerifiedStatus
        {
            No,
            Yes
        }

        #endregion

        public static Tile CreateTile(IDataWorker dataWorker, Team team,
            BingoTask task, int boardIndex, VerifiedStatus verifiedStatus = VerifiedStatus.No) =>
            dataWorker.Tiles.Create(team, task, boardIndex, verifiedStatus);

        public static bool IsVerified(this Tile tile) =>
            tile.Verified == 1;

        public static bool IsNotVerified(this Tile tile) =>
            tile.Verified != 1;

        public static void ChangeTask(this Tile tile, BingoTask task) =>
            tile.Task = task;

        public static void SwapBoardIndex(this Tile tile, Tile otherTile)
        {
            // TODO: JR - fix this
            //BingoTask otherTileTask = otherTile.Task;
            //otherTile.Task = tile.Task;
            //tile.Task = otherTileTask;
            int otherTileBoardIndex = otherTile.BoardIndex;
            otherTile.BoardIndex = tile.BoardIndex;
            tile.BoardIndex = otherTileBoardIndex;
        }

        public static Tile? GetWithTeamAndBoardIndex(IDataWorker dataWorker, Team team, int boardIndex) =>
            team.Tiles.FirstOrDefault(t => t.BoardIndex == boardIndex);
    }
}