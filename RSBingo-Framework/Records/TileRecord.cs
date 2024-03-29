﻿// <copyright file="TileRecord.cs" company="PlaceholderCompany">
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

        public static bool IsVerified(this Tile tile) =>
            tile.IsVerified == 1;

        public static bool IsNotVerified(this Tile tile) =>
            tile.IsVerified != 1;

        public static void ChangeTask(this Tile tile, BingoTask task) =>
            tile.Task = task;

        public static Tile? GetWithTeamAndBoardIndex(IDataWorker dataWorker, Team team, int boardIndex) =>
            team.Tiles.FirstOrDefault(t => t.BoardIndex == boardIndex);

        public static void SwapTasks(this Tile tileOne, Tile tileTwo, IDataWorker dataWorker) => dataWorker.Tiles.SwapTasks(tileOne, tileTwo);

        public static Evidence? GetEvidence(this Tile tile, IDataWorker dataWorker, ulong discordUserId) =>
            tile.Evidence.FirstOrDefault(e => e.DiscordUserId == discordUserId);

        public static bool IsCompleteAsBool(this Tile tile) =>
            tile.IsComplete == (sbyte)CompleteStatus.Yes;

        public static void SetCompleteStatus(this Tile tile, CompleteStatus completeStatus) =>
            tile.IsComplete = (sbyte)completeStatus;
    }
}