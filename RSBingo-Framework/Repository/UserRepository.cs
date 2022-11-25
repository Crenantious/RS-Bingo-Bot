// <copyright file="UserRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Repository
{
    using Microsoft.EntityFrameworkCore;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Interfaces.IRepository;
    using RSBingo_Framework.Models;

    /// <summary>
    /// Class detailing use of <see cref="User"/> as a repository.
    /// </summary>
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dataWorker">Reference to the <see cref="IDataWorker"/>.</param>
        public UserRepository(IDataWorker dataWorker)
            : base(dataWorker) { }

        /// <inheritdoc/>
        public override User Create()
        {
            return Add(new User());
        }

        public User Create(ulong discordId, string teamName)
        {
            Team? team = DataWorker.Teams.GetByName(teamName);

            if (team == null)
            {
                throw new NullReferenceException("The given team does not exist.");
            }

            return Create(discordId, team);
        }

        public User Create(ulong discordId, Team team) => Add(new User()
        {
            DiscordUserId = discordId,
            Team = team
        });

        public int Delete(ulong discordUserId)
        {
            User? user = GetByDiscordId(discordUserId);
            if (user != null)
            {
                Remove(user);
                return 0;
            }
            return -1;
        }

        public int Delete(User user)
        {
            Remove(user);
            return 0;
        }

        public User? GetByDiscordId(ulong discordId) =>
            FirstOrDefault(u => u.DiscordUserId == discordId);

        public IEnumerable<User> GetAllUsers() =>
            GetAll();

        public bool Exists(ulong discordId) =>
            GetByDiscordId(discordId) == null;

        public int ChangeTeam(User user, string newTeam)
        {
            Team? team = DataWorker.Teams.GetByName(newTeam);
            if (team == null) { return -1; }
            user.Team = team;
            return 0;
        }

        public int ChangeTeam(ulong discordId, string newTeam)
        {
            User? user = GetByDiscordId(discordId);
            if (user == null) { return -1; }
            ChangeTeam(user, newTeam);
            return 0;
        }

        /// <inheritdoc/>
        public override void LoadCascadeNavigations(User user)
        {
            DataWorker.Users.Where(u => u.DiscordUserId == user.DiscordUserId)
                .Include(t => t.Evidence);
        }
    }
}
