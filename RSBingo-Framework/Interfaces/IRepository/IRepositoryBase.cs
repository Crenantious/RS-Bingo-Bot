// <copyright file="IRepositoryBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Interfaces.IRepository
{
    using RSBingo_Framework.Models;

    /// <summary>
    /// Wrapper around the DbSet implementing the Repository Pattern and providing common access methods.
    /// </summary>
    public interface IRepositoryBase
    {
        /// <summary>
        /// Gets reference to the DataWorker that the DbSet will call upon.
        /// </summary>
        IDataWorker DataWorker { get; }

        /// <summary>
        /// Diverted reference to the DBSet Find method of the given Model type.
        /// </summary>
        /// <param name="id">Finds the entity of the given primary key value.</param>
        /// <returns>The entity if found.</returns>
        BingoRecord? FindByPK(object id);
    }
}
