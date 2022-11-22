// <copyright file="IUserRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Interfaces.IRepository
{
    using RSBingo_Framework.Models;

    /// <summary>
    /// Interface detailing use of <see cref="User"/>as a repository.
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        public User Create(ulong discordId, string teamName);
        public User Create(ulong discordId, Team team);
        public User? GetByDiscordId(ulong discordId);
        public IEnumerable<User> GetAllUsers();
        public int Delete(ulong discordUserId);
        public int Delete(User user);
        public bool Exists(ulong discordId);
        public int ChangeTeam(User user, string newTeam);
        public int ChangeTeam(ulong discordId, string newTeam);
    }
}
