// <copyright file="TileRecord.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Records
{
    using RSBingo_Common;
    using RSBingo_Framework.DAL;
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

        public static bool IsVerified(this Tile tile) =>
            tile.Verified == 1;

        public static bool IsNotVerified(this Tile tile) =>
            tile.Verified != 1;

        public static void ChangeTask(this Tile tile, BingoTask task)
        {
            
            if (tile.Task.IsNoTask())
            {
                DataFactory.AvailableNoTasks[tile.Team.RowId].Add(tile.Task);
            }

            if (task.IsNoTask())
            {
                DataFactory.AvailableNoTasks[tile.Team.RowId].Remove(task);
            }

            tile.TaskId = task.RowId;
        }

        public static void SetToNoTask(this Tile tile)
        {
            ChangeTask(tile, DataFactory.AvailableNoTasks[tile.Team.RowId][0]);
        }
    }
}