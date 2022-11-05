// <copyright file="BingoTaskRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Repository
{
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Interfaces.IRepository;
    using RSBingo_Framework.Models;
    using static RSBingo_Framework.Records.BingoTaskRecord;

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
                Difficulty = difficulty
            });

        public void DeleteAll() =>
            RemoveRange(GetAll());
    }
}
