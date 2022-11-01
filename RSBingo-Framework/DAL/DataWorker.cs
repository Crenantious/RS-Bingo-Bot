// <copyright file="DataWorker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DAL
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Interfaces.IRepository;
    using RSBingo_Framework.Repository;

    /// <summary>
    /// Class for the DataWorker class which wraps around the DBContext giving a transaction scope for caching and updates.
    /// </summary>
    public class DataWorker : IDataWorker
    {
        private DateTime? dataWorkerDateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataWorker"/> class.
        /// </summary>
        /// <param name="context">The database conext.</param>
        /// <param name="logger">Reference to the logger</param>
        public DataWorker(RSBingoContext context, ILogger<DataWorker> logger)
        {
            Context = context;
            Logger = logger;
            BingoTasks = new BingoTaskRepository(this);
            Evidence = new EvidenceRepository(this);
            Restrictions = new RestrictionRepository(this);
            Teams = new TeamRepository(this);
            Tiles = new TileRepository(this);
            Users = new UserRepository(this);
        }

        /// <summary>
        /// Gets the current time.
        /// </summary>
        public static DateTime DateTimeNow => DateTime.Now; // TODO: We may need toi truncate this to the second, return dateTime.Truncate(TimeSpan.FromSeconds(1));. This would be in a Extension class which would have to live in a "Common" project.

        /// <inheritdoc/>
        public RSBingoContext Context { get; }

        /// <inheritdoc/>
        public bool TrackChanges
        {
            get => Context.ChangeTracker.QueryTrackingBehavior != QueryTrackingBehavior.NoTracking;
            set => Context.ChangeTracker.QueryTrackingBehavior = value ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking;
        }

        /// <inheritdoc />
        public DateTime DataWorkerDateTimeNow
        {
            get
            {
                // The current date and time that can be used within the database fields.
                return DateTimeNow;
            }
        }

        /// <inheritdoc />
        public DateTime DataWorkerDateTime
        {
            get
            {
                if (!dataWorkerDateTime.HasValue)
                {
                    dataWorkerDateTime = DataWorkerDateTimeNow;
                }

                return dataWorkerDateTime.Value;
            }
        }

        /// <inheritdoc/>
        public ILogger<IDataWorker> Logger { get; }

        /// <inheritdoc/>
        public IBingoTaskRepository BingoTasks { get; set; }

        /// <inheritdoc/>
        public IEvidenceRepository Evidence { get; set; }

        /// <inheritdoc/>
        public IRestrictionRepository Restrictions { get; set; }


        /// <inheritdoc/>
        public ITeamRepository Teams { get; set; }

        /// <inheritdoc/>
        public ITileRepository Tiles { get; set; }

        /// <inheritdoc/>
        public IUserRepository Users { get; set; }

        /// <inheritdoc/>
        public void Dispose() => Context.Dispose();

        /// <inheritdoc/>
        public void RollBack()
        {
            Context.RollBack();
        }

        /// <inheritdoc/>
        public int SaveChanges(bool reset = false)
        {
            return Context.SaveChanges();
        }
    }
}
