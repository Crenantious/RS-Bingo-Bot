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

        private static readonly EnumDict<VerifiedStatus> VerifiedLookup = new EnumDict<VerifiedStatus>(VerifiedStatus.No)
            .Add(VerifiedStatus.Yes, 1);

        private static readonly EnumDict<CompleteStatus> CompleteLookup = new EnumDict<CompleteStatus>(CompleteStatus.No)
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

        public static bool IsVerified(this Tile tile)
        {
            var verificationEvidence = tile.Evidence.GetVerificationEvidence();
            var acceptedVerificationEvidence = verificationEvidence.GetAcceptedEvidence();
            return verificationEvidence.Count() == acceptedVerificationEvidence.Count();
        }

        public static bool IsCompleteAsBool(this Tile tile) =>
            CompleteLookup.Get(tile.IsComplete) == CompleteStatus.Yes;

        public static void SetCompleteStatus(this Tile tile, CompleteStatus completeStatus) =>
            tile.IsComplete = (sbyte)completeStatus;
    }
}