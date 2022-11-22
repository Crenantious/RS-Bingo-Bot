// <copyright file="TileRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Repository
{
    using RSBingo_Framework.DAL;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Interfaces.IRepository;
    using RSBingo_Framework.Models;
    using RSBingo_Framework.Records;
    using static RSBingo_Framework.Records.TileRecord;

    /// <summary>
    /// Class detailing use of <see cref="Tile"/> as a repository.
    /// </summary>
    public class TileRepository : RepositoryBase<Tile>, ITileRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TileRepository"/> class.
        /// </summary>
        /// <param name="dataWorker">Reference to the dataworker.</param>
        public TileRepository(IDataWorker dataWorker)
            : base(dataWorker) { }

        /// <inheritdoc/>
        public override Tile Create()
        {
            return Add(new Tile());
        }

        public Tile Create(Team team, BingoTask task, VerifiedStatus verifiedStatus = VerifiedStatus.No)
        {
            Tile tile = Create();
            tile.Verified = (sbyte)verifiedStatus;

            tile.Task = task;

            return tile;
        }

        public Tile? GetById(int id) =>
            FirstOrDefault(t => t.RowId == id);

        public IEnumerable<Tile> GetByIds(IEnumerable<int> ids) =>
            Where(t => ids.Contains(t.RowId)).ToList();

        public IEnumerable<Tile> GetByTaskId(int id) =>
            Where(t => t.TaskId == id);

        public IEnumerable<Tile> GetByTaskIds(IEnumerable<int> ids) =>
            Where(t => ids.Contains(t.TaskId));

        public IEnumerable<Tile> GetByTasks(IEnumerable<BingoTask> tasks) =>
            GetByTaskIds(tasks.Select(t => t.RowId));

        public Tile? GetByTeamAndTaskId(Team team, int taskId) =>
            team.Tiles.FirstOrDefault(t => t.TaskId == taskId);

        public IEnumerable<Tile> GetAllTiles() =>
            GetAll();

        public void SetToNoTask(Tile tile)
        {
            tile.SetToNoTask();
        }

        public void SetToNoTask(IEnumerable<Tile> tiles)
        {
            foreach (Tile tile in tiles)
            {
                SetToNoTask(tile);
            }
        }

        public void ChangeTask(Tile tile, BingoTask task) =>
            tile.ChangeTask(task);

        public void SwapTasks(Tile tile1, Tile tile2)
        {
            BingoTask tile1Task = tile1.Task;
            BingoTask tile2Task = tile2.Task;
            tile1.SetToNoTask();
            tile2.SetToNoTask();
            DataWorker.SaveChanges();
            tile1.ChangeTask(tile2Task);
            tile2.ChangeTask(tile1Task);
        }

        public void Delete(Tile tile) =>
            Remove(tile);

        public void DeleteMany(IEnumerable<Tile> tiles) =>
            RemoveRange(tiles);
    }
}