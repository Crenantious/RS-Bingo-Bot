// <copyright file="IBingoTaskRestrictionRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Interfaces.IRepository
{
    using RSBingo_Framework.Models;

    /// <summary>
    /// Interface detailing use of <see cref="BingoTask"/>as a repository.
    /// </summary>
    public interface IBingoTaskRestrictionRepository
    {
        public BingoTaskRestriction Create(int taskId, int restrictionId);
    }
}
