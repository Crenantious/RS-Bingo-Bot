// <copyright file="IDataWorker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Interfaces
{
    using Microsoft.Extensions.Logging;
    using RSBingo_Framework.DAL;
    using RSBingo_Framework.Interfaces.IRepository;
    using RSBingo_Framework.Models;

    /// <summary>
    /// Interface for the DataWorker class which wraps around the DBContext giving a transaction scope for caching and updates.
    /// </summary>
    public interface IDataWorker : IDisposable
    {
        /// <summary>
        /// Gets reference to the Context we are wrapping.
        /// </summary>
        RSBingoContext Context { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the ability for the context to track changes should be turned on or not.
        /// </summary>
        bool TrackChanges { get; set; }

        /// <summary>
        /// Gets logging interface common to all DI created objects. Usually private, except when inherited.
        /// </summary>
        ILogger<IDataWorker> Logger { get; }

        /// <summary>
        /// Gets a date time that is current and formatted for a database field.
        /// </summary>
        public DateTime DataWorkerDateTimeNow { get; }

        /// <summary>
        /// Gets a date that is created once on initial request and kept locked during the lifetime of the data worker.
        /// </summary>
        public DateTime DataWorkerDateTime { get; }

        /// <summary>
        /// Gets or sets link to the repository for <see cref="BingoTask"/> entities.
        /// </summary>
        IBingoTaskRepository BingoTasks { get; set; }

        /// <summary>
        /// Gets or sets link to the repository for <see cref="Models.Evidence"/> entities.
        /// </summary>
        IEvidenceRepository Evidence { get; set; }

        /// <summary>
        /// Gets or sets link to the repository for <see cref="Restriction"/> entities.
        /// </summary>
        IRestrictionRepository Restrictions { get; set; }

        /// <summary>
        /// Gets or sets link to the repository for <see cref="BingoTaskRestriction"/> entities.
        /// </summary>
        IBingoTaskRestrictionRepository BingoTaskRestriction { get; set; }

        /// <summary>
        /// Gets or sets link to the repository for <see cref="Team"/> entities.
        /// </summary>
        ITeamRepository Teams { get; set; }

        /// <summary>
        /// Gets or sets link to the repository for <see cref="Tile"/> entities.
        /// </summary>
        ITileRepository Tiles { get; set; }

        /// <summary>
        /// Gets or sets link to the repository for <see cref="User"/> entities.
        /// </summary>
        IUserRepository Users { get; set; }

        /// <summary>
        /// Request to save any changes to entities value or state.
        /// </summary>
        /// <param name="reset">Flag to reset the worker state to allow re-use.</param>
        /// <returns>The number of entities created/updated/deleted.</returns>
        int SaveChanges(bool reset = false);

        /// <summary>
        /// Revert any changes to entities being tracked by to their state at the last save.
        /// This does not reset entities being held outside the context.
        /// </summary>
        void RollBack();
    }
}
