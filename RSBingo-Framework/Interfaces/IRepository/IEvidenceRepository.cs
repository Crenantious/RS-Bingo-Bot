// <copyright file="IEvidenceRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Interfaces.IRepository
{
    using RSBingo_Framework.Models;
    using static RSBingo_Framework.Records.EvidenceRecord;

    /// <summary>
    /// Interface detailing use of <see cref="Evidence"/>as a repository.
    /// </summary>
    public interface IEvidenceRepository : IRepository<Evidence>
    {
        public Evidence Create(User user, Tile tile, string url, EvidenceType type);
        public Evidence? GetByTile(Tile tile);
    }
}
