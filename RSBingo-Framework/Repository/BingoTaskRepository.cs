// <copyright file="BingoTaskRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Repository
{
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Interfaces.IRepository;
    using RSBingo_Framework.Models;
    using static RSBingo_Framework.Records.BingoTaskRecord;
    using static RSBingo_Common.General;

    /// <summary>
    /// Class detailing use of <see cref="BingoTask"/> as a repository.
    /// </summary>
    public class BingoTaskRepository : RepositoryBase<BingoTask>, IBingoTaskRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BingoTaskRepository"/> class.
        /// </summary>
        /// <param name="dataWorker">Reference to the dataworker.</param>
        public BingoTaskRepository(IDataWorker dataWorker)
            : base(dataWorker) { }

        /// <inheritdoc/>
        public override BingoTask Create()
        {
            return Add(new BingoTask());
        }

        public BingoTask Create(string name, Difficulty difficulty) =>
            Add(new BingoTask()
            {
                Name = name,
                Difficulty = (sbyte)difficulty,
            });

        public IEnumerable<BingoTask> CreateMany(string name, Difficulty difficulty, int amount)
        {
            if (amount < 1) { throw new ArgumentOutOfRangeException(nameof(amount)); }

            List<BingoTask> tasks = new();

            for (int i = 0; i < amount; i++)
            {
                Add(Create(name, difficulty));
            }

            return tasks;
        }

        public IEnumerable<BingoTask> GetByName(string name) =>
           Where(t => t.Name == name);

        public IEnumerable<BingoTask> GetByNameAndDifficulty(string name, Difficulty difficulty) =>
           Where(t => t.Name == name && t.Difficulty == (sbyte)difficulty);

        public BingoTask? GetById(int id) =>
           FirstOrDefault(t => t.RowId == id);

        public IEnumerable<BingoTask> GetAllTasks() =>
            GetAll();

        public IEnumerable<BingoTask> GetAllWithDifficulty(Difficulty difficulty) =>
            Where(t => t.Difficulty == (sbyte)difficulty).ToList();

        public void Delete(string name, Difficulty difficulty)
        {
            BingoTask? task = FirstOrDefault(t => t.Name == name && t.Difficulty == (sbyte)difficulty);
            if (task != null) { Delete(task); }
        }

        public void Delete(BingoTask bingoTask) =>
            Remove(bingoTask);

        /// <summary>
        /// Deletes as many <see cref="BingoTask"/>s that can be found with matching <paramref name="name"/>
        /// and <paramref name="difficulty"/> up to the <paramref name="amount"/>.<br/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="difficulty"></param>
        /// <param name="amount"></param>
        public void DeleteMany(string name, Difficulty difficulty, int amount)
        {
            IEnumerable<BingoTask> tasks = Where(t => t.Name == name && t.Difficulty == (sbyte)difficulty).AsEnumerable();
            for (int i = 0; i < MathF.Min(tasks.Count(), amount); i++)
            {
                Delete(tasks.ElementAt(i));
            }
        }

        public void DeleteMany(IEnumerable<BingoTask> bingoTasks) =>
            RemoveRange(bingoTasks);

        public void DeleteAll() =>
            RemoveRange(GetAll());
    }
}
