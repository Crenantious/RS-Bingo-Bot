// <copyright file="TeamRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Repository
{
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Interfaces.IRepository;
    using RSBingo_Framework.Models;

    /// <summary>
    /// Class detailing use of <see cref="Team"/> as a repository.
    /// </summary>
    public class TeamRepository : RepositoryBase<Team>, ITeamRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamRepository"/> class.
        /// </summary>
        /// <param name="dataWorker">Reference to the dataworker.</param>
        public TeamRepository(IDataWorker dataWorker)
            : base(dataWorker) { }

        /// <inheritdoc/>
        public override Team Create() =>
            Add(new Team());

        public Team Create(string name, ulong boardChannelId) =>
            Add(new Team()
            {
                Name = name,
                BoardChannelId = boardChannelId,
            });

        public int Delete(string name)
        {
            Team? team = GetByName(name);
            if (team != null)
            {
                foreach (User user in team.Users)
                {
                    DataWorker.Users.Delete(user);
                }

                foreach (Tile tile in team.Tiles)
                {
                    DataWorker.Tiles.Delete(tile);
                }

                Remove(team);
                return 0;
            }
            return -1;
        }

        public bool DoesTeamExist(string name) =>
            GetByName(name) != null;

        public Team? GetByName(string name) =>
            FirstOrDefault(t => t.Name == name);

        public IEnumerable<Team> GetTeams() =>
            GetAll();
    }
}
