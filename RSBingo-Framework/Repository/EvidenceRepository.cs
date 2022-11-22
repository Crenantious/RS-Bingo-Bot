// <copyright file="EvidenceRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Repository
{
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Interfaces.IRepository;
    using RSBingo_Framework.Models;
    using static RSBingo_Framework.Records.EvidenceRecord;

    /// <summary>
    /// Class detailing use of <see cref="Evidence"/> as a repository.
    /// </summary>
    public class EvidenceRepository : RepositoryBase<Evidence>, IEvidenceRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvidenceRepository"/> class.
        /// </summary>
        /// <param name="dataWorker">Refernce to the dataworker.</param>
        public EvidenceRepository(IDataWorker dataWorker)
            : base(dataWorker) { }

        /// <inheritdoc/>
        public override Evidence Create() =>
            Add(new Evidence());

        public Evidence Create(User user, Tile tile, string url, EvidenceType type)
        {
            Evidence evidence = Create();
            evidence.DiscordUser = user;
            evidence.Tile = tile;
            evidence.Url = url;
            evidence.Type = (sbyte)type;

            user.Evidence.Add(evidence);

            return evidence;
        }

        public Evidence? GetById(int rowId) => FirstOrDefault(e => e.RowId == rowId);
    }
}
