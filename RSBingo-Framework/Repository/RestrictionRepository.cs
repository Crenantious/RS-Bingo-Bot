// <copyright file="RestrictionRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Repository
{
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Interfaces.IRepository;
    using RSBingo_Framework.Models;

    /// <summary>
    /// Class detailing use of <see cref="Restrciton"/> as a repository.
    /// </summary>
    public class RestrictionRepository : RepositoryBase<Restrciton>, IRestrictionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestrictionRepository"/> class.
        /// </summary>
        /// <param name="dataWorker">Refernce to the dataworker.</param>
        public RestrictionRepository(IDataWorker dataWorker)
            : base(dataWorker) { }

        /// <inheritdoc/>
        public override Restrciton Create()
        {
            return Add(new Restrciton());
        }
    }
}
