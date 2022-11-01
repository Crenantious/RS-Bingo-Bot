// <copyright file="UserRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Repository
{
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
        /// <param name="dataWorker">Refernce to the dataworker.</param>
        public UserRepository(IDataWorker dataWorker)
            : base(dataWorker) { }

        /// <inheritdoc/>
        public override User Create()
        {
            return Add(new User());
        }

        public User? GetByDiscordId(ulong discordId) =>
            FirstOrDefault(u => u.DiscordUserId == discordId);
    }
}
