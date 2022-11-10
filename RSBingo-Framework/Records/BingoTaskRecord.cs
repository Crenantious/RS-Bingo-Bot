// <copyright file="BingoTaskRecord.cs" company="PlaceholderCompany">
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

        private static readonly EnumDict<Difficulty> DifficultyLookup = new EnumDict<Difficulty>(Difficulty.Easy)
            .Add(Difficulty.Medium, 1)
            .Add(Difficulty.Hard, 2);

        public enum Difficulty
        {
            Easy,
            Medium,
            Hard
        }

        #endregion

        public static BingoTask CreateBingoTask(IDataWorker dataWorker, string name, Difficulty type) =>
            dataWorker.BingoTasks.Create(name, type);
    }
}
