// <copyright file="ITeamRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Interfaces.IRepository
{
    using RSBingo_Framework.Models;

    /// <summary>
    /// Interface detailing use of <see cref="Team"/>as a repository.
    /// </summary>
    public interface ITeamRepository
    {
        public Team Create(string name, ulong boardChannelId);
        public int Delete(string name);
        public bool DoesTeamExist(string name);
        public Team? GetByName(string name);
        public IEnumerable<Team> GetTeams();
    }
}