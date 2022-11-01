// <copyright file="IUserRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Interfaces.IRepository
{
    using RSBingo_Framework.Models;

    /// <summary>
    /// Interface detailing use of <see cref="User"/>as a repository.
    /// </summary>
    public interface IUserRepository
    {
        public User? GetByDiscordId(ulong discordId);
    }
}
