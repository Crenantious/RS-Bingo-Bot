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

        public Tile Create(string teamName, int taskId, int boardIndex, VerifiedStatus verifiedStatus = VerifiedStatus.No)
        {
            BingoTask? task = DataWorker.BingoTasks.GetById(taskId);
            if (task == null) { throw new NullReferenceException($"Could not find task with id {taskId}."); }

            return Create(teamName, task, boardIndex, verifiedStatus);
        }

        public Tile Create(string teamName, BingoTask task, int boardIndex, VerifiedStatus verifiedStatus = VerifiedStatus.No)
        {
            Team? team = DataWorker.Teams.GetByName(teamName);
            if (team == null) { throw new NullReferenceException($"Could not find team with name {teamName}."); }

            return Create(team, task, boardIndex, verifiedStatus);
        }

        public Tile Create(Team team, BingoTask task, int boardIndex, VerifiedStatus verifiedStatus = VerifiedStatus.No) =>
            Create(team.RowId, task.RowId, boardIndex, verifiedStatus);

        public Tile Create(int teamId, int taskId, int boardIndex, VerifiedStatus verifiedStatus = VerifiedStatus.No)
        {
            return Add(new Tile()
            {
                TeamId = teamId,
                TaskId = taskId,
                Verified = (sbyte)verifiedStatus,
                BoardIndex = boardIndex
            });
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

        public void ChangeTask(Tile tile, BingoTask task) =>
            tile.ChangeTask(task);

        public void SwapTasks(Tile tile1, Tile tile2)
        {
            // TODO: JR - fix this
            BingoTask tile1Task = tile1.Task;
            tile1.Task = tile2.Task;
            tile2.Task = tile1Task;
            //BingoTask tile2Task = tile2.Task;
            ////tile1.SetToNoTask();
            ////tile2.SetToNoTask();
            ////DataWorker.SaveChanges();
            //tile1.ChangeTask(tile2Task);
            //tile2.ChangeTask(tile1Task);
        }

        public void Delete(Tile tile) =>
            Remove(tile);

        public void DeleteMany(IEnumerable<Tile> tiles) =>
            RemoveRange(tiles);
    }
}