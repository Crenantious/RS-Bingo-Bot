﻿// <copyright file="BingoTaskRecord.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Records
{
    using RSBingo_Common;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using static RSBingo_Framework.Repository.BingoTaskRepository;

    public static class BingoTaskRecord
    {
        #region enums & lookups

        private static readonly EnumDict<Difficulty> DifficultyLookup = new EnumDict<Difficulty>(Difficulty.Undefined)
            .Add(Difficulty.Easy, 1)
            .Add(Difficulty.Medium, 2)
            .Add(Difficulty.Hard, 3);

        public enum Difficulty
        {
            Undefined,
            Easy,
            Medium,
            Hard
        }

        #endregion

        public static BingoTask CreateBingoTask(IDataWorker dataWorker, string name, Difficulty type) =>
            dataWorker.BingoTasks.Create(name, type);

        public static IEnumerable<BingoTask> GetAllBingoTasks(IDataWorker dataWorker) =>
            dataWorker.BingoTasks.GetAll();

        public static Difficulty GetDifficutyAsDifficulty(this BingoTask bingoTask) =>
            DifficultyLookup.Get(bingoTask.Difficulty);
    }
}
