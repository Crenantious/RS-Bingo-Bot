// <copyright file="TaskRestrcitonRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Repository
{
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Interfaces.IRepository;
    using RSBingo_Framework.Models;

    /// <summary>
    /// Class detailing use of <see cref="TaskRestrciton"/> as a repository.
    /// </summary>
    public class TaskRestrcitonRepository : RepositoryBase<TaskRestrciton>, ITaskRestrcitonRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRestrcitonRepository"/> class.
        /// </summary>
        /// <param name="dataWorker">Refernce to the dataworker.</param>
        public TaskRestrcitonRepository(IDataWorker dataWorker)
            : base(dataWorker) { }

        /// <inheritdoc/>
        public override TaskRestrciton Create()
        {
            return Add(new TaskRestrciton());
        }
    }
}
