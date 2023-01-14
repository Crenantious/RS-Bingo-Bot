

namespace RSBingo_Framework.Records
{
    using RSBingo_Common;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static RSBingo_Framework.Repository.EvidenceRepository;

    public static class EvidenceRecord
    {
        #region enums & lookups

        public enum EvidenceType
        {
            Undefined,
            TileVerification,
            Drop,
        }

        private static readonly EnumDict<EvidenceType> EvidenceTypeLookup = new EnumDict<EvidenceType>(EvidenceType.Undefined)
         .Add(EvidenceType.TileVerification, 1)
         .Add(EvidenceType.Drop, 2);

        #endregion

        public static Evidence CreateEvidence(IDataWorker dataWorker, User user, Tile tile, string url, EvidenceType type) =>
            dataWorker.Evidence.Create(user, tile, url, type);

        public static Evidence? GetByTile(IDataWorker dataWorker, Tile tile, User user, EvidenceType evidenceType) =>
            dataWorker.Evidence.FirstOrDefault(e => e.Tile == tile && e.DiscordUser == user && e.EvidenceType == (sbyte)evidenceType);
    }
}
