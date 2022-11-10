// <copyright file="IBingoTaskRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Interfaces.IRepository
{
    using RSBingo_Framework.Models;
    using static RSBingo_Framework.Records.BingoTaskRecord;

    /// <summary>
    /// Interface detailing use of <see cref="BingoTask"/>as a repository.
    /// </summary>
    public interface IBingoTaskRepository
    {
        public BingoTask Create(string name, Difficulty difficulty);
        public IEnumerable<BingoTask> CreateMany(string name, Difficulty difficulty, int amount);
        public BingoTask? GetByName(string name);
        public BingoTask? GetById(int id);
        public IEnumerable<BingoTask> GetAllTasks();
        public IEnumerable<BingoTask> GetAllWithDifficulty(Difficulty difficulty);
        public void Delete(string name, Difficulty difficulty);
        public void Delete(BingoTask bingoTask);
        public void DeleteMany(string name, Difficulty difficulty, int amount);
        public void DeleteAll();
    }
}
