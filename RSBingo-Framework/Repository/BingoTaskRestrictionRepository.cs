// <copyright file="BingoTaskRestrictionRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Repository
{
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Interfaces.IRepository;
    using RSBingo_Framework.Models;
    using System.Threading.Tasks;

    /// <summary>
    /// Class detailing use of <see cref="BingoTaskRestriction"/> as a repository.
    /// </summary>
    public class BingoTaskRestrictionRepository : RepositoryBase<BingoTaskRestriction>, IBingoTaskRestrictionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BingoTaskRestrictionRepository"/> class.
        /// </summary>
        /// <param name="dataWorker">Reference to the dataworker.</param>
        public BingoTaskRestrictionRepository(IDataWorker dataWorker)
            : base(dataWorker) { }

        /// <inheritdoc/>
        public override BingoTaskRestriction Create()
        {
            return Add(new BingoTaskRestriction());
        }

        public BingoTaskRestriction Create(int taskId, int restrictionId) =>
            Add(new BingoTaskRestriction()
            {
                TaskId = taskId,
                RestrictionId = restrictionId
            });
    }
}
