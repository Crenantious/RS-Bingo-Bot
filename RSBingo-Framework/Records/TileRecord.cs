// <copyright file="TileRecord.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Records
{
    using RSBingo_Common;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;

    public static class TileRecord
    {
        #region enums & lookups

        public static readonly EnumDict<VerifiedStatus> VerifiedLookup = new EnumDict<VerifiedStatus>(VerifiedStatus.No)
            .Add(VerifiedStatus.Yes, 1);

        public static readonly EnumDict<CompleteStatus> CompleteLookup = new EnumDict<CompleteStatus>(CompleteStatus.No)
            .Add(CompleteStatus.Yes, 1);

        public enum VerifiedStatus
        {
            No,
            Yes
        }

        public enum CompleteStatus
        {
            No,
            Yes
        }

        #endregion

        public static Tile CreateTile(IDataWorker dataWorker, Team team,
            BingoTask task, int boardIndex, VerifiedStatus verifiedStatus = VerifiedStatus.No) =>
            dataWorker.Tiles.Create(team, task, boardIndex, verifiedStatus);

        public static void ChangeTask(this Tile tile, BingoTask task) =>
            tile.Task = task;

        public static void SwapTasks(this Tile tileOne, Tile tileTwo, IDataWorker dataWorker) =>
            dataWorker.Tiles.SwapTasks(tileOne, tileTwo, dataWorker);

        public static bool IsVerified(this Tile tile) =>
            VerifiedLookup.Get(tile.IsVerified) == VerifiedStatus.Yes;

        public static bool IsCompleteAsBool(this Tile tile) =>
            CompleteLookup.Get(tile.IsComplete) == CompleteStatus.Yes;

        /// <summary>
        /// Sets <see cref="Tile.IsVerified"/> based on the <see cref="Evidence"/> submitted for <paramref name="tile"/>.
        /// </summary>
        public static void UpdateVerifiedStatus(this Tile tile)
        {
            // Checks if the tile has accepted verification evidence from every user on the team.
            bool isVerified = tile.Team.Users
                .All(u => u.Evidence.GetVerificationEvidence()
                            .GetAcceptedEvidence()
                            .Any());

            var verifiedStatus = isVerified ?
                                 TileRecord.VerifiedStatus.Yes :
                                 TileRecord.VerifiedStatus.No;
            tile.SetVerifiedStatus(verifiedStatus);
        }

        /// <summary>
        /// Sets <see cref="Tile.IsComplete"/> based on the <see cref="Evidence"/> submitted for <paramref name="tile"/>.
        /// </summary>
        public static void UpdateCompleteStatus(this Tile tile)
        {
            // Checks if the tile has an accepted drop evidence from any user.
            bool isComplete = tile.Evidence.GetDropEvidence()
                                .GetAcceptedEvidence()
                                .Any();

            var completeStatus = isComplete ?
                                 TileRecord.CompleteStatus.Yes :
                                 TileRecord.CompleteStatus.No;
            tile.SetCompleteStatus(completeStatus);
        }

        public static void SetVerifiedStatus(this Tile tile, VerifiedStatus verifiedStatus) =>
            tile.IsVerified = VerifiedLookup.Get(verifiedStatus);

        public static void SetCompleteStatus(this Tile tile, CompleteStatus completeStatus) =>
            tile.IsComplete = CompleteLookup.Get(completeStatus);
    }
}