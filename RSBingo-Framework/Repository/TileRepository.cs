﻿// <copyright file="TileRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Repository
{
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Interfaces.IRepository;
    using RSBingo_Framework.Models;

    /// <summary>
    /// Class detailing use of <see cref="Tile"/> as a repository.
    /// </summary>
    public class TileRepository : RepositoryBase<Tile>, ITileRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TileRepository"/> class.
        /// </summary>
        /// <param name="dataWorker">Refernce to the dataworker.</param>
        public TileRepository(IDataWorker dataWorker)
            : base(dataWorker) { }

        /// <inheritdoc/>
        public override Tile Create()
        {
            return Add(new Tile());
        }

        public List<Tile> GetByIds(IEnumerable<int> ids) =>
            Where(t => ids.Contains(t.RowId)).ToList();
    }
}
