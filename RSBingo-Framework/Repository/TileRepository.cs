// <copyright file="TileRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Repository
{
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Interfaces.IRepository;
    using RSBingo_Framework.Models;
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

        public Tile CreateTile(string teamName, string taskName, VerifiedStatus verifiedStatus)
        {
            BingoTask? task = DataWorker.BingoTasks.GetByName(teamName);
            if (task == null) { throw new NullReferenceException($"Could not find task with name {taskName}."); }

            return CreateTile(teamName, task, verifiedStatus);
        }

        public Tile CreateTile(string teamName, BingoTask task, VerifiedStatus verifiedStatus)
        {
            Team? team = DataWorker.Teams.GetByName(teamName);
            if (team == null) { throw new NullReferenceException($"Could not find team with name {teamName}."); }

            return Add(new Tile()
            {
                TeamId = team.RowId,
                TaskId = task.RowId,
                Verified = (sbyte)verifiedStatus
            });
        }

        public IEnumerable<Tile> GetByIds(IEnumerable<int> ids) =>
            Where(t => ids.Contains(t.RowId)).ToList();

        public IEnumerable<Tile> GetAllTiles() =>
            GetAll();
    }
}
