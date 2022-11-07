﻿// <copyright file="ITileRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Interfaces.IRepository
{
    using RSBingo_Framework.Models;
    using static RSBingo_Framework.Records.TileRecord;

    /// <summary>
    /// Interface detailing use of <see cref="Tile"/>as a repository.
    /// </summary>
    public interface ITileRepository
    {
        public IEnumerable<Tile> GetByIds(IEnumerable<int> ids);
        public IEnumerable<Tile> GetAllTiles();
        public Tile CreateTile(string teamName, string taskName, VerifiedStatus verifiedStatus);
        public Tile CreateTile(string teamName, BingoTask task, VerifiedStatus verifiedStatus);
    }
}
