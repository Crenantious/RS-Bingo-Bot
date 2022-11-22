// <copyright file="ITileRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Interfaces.IRepository
{
    using RSBingo_Framework.Models;
    using static RSBingo_Framework.Records.TileRecord;

    /// <summary>
    /// Interface detailing use of <see cref="Tile"/>as a repository.
    /// </summary>
    public interface ITileRepository : IRepository<Tile>
    {
        public Tile Create(Team team, BingoTask task, VerifiedStatus verifiedStatus = VerifiedStatus.No);
        public Tile? GetById(int id);
        public IEnumerable<Tile> GetByIds(IEnumerable<int> ids);
        public IEnumerable<Tile> GetByTaskId(int id);
        public IEnumerable<Tile> GetByTaskIds(IEnumerable<int> ids);

        /// <summary>
        /// Gets all the tiles that have a task that is in <paramref name="tasks"/>.
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public IEnumerable<Tile> GetByTasks(IEnumerable<BingoTask> tasks);
        public Tile? GetByTeamAndTaskId(Team team, int taskId);
        public IEnumerable<Tile> GetAllTiles();
        public void SetToNoTask(Tile tile);

        public void SetToNoTask(IEnumerable<Tile> tiles);
        public void ChangeTask(Tile tile, BingoTask task);
        public void SwapTasks(Tile tile1, Tile tile2);

        public void Delete(Tile tile);
        public void DeleteMany(IEnumerable<Tile> tiles);
    }
}
