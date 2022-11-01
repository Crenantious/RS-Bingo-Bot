

namespace RSBingo_Framework.Records
{
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
        public static Evidence CreateEvidence(IDataWorker dataWorker, User user, Tile tile, string url) =>
            dataWorker.Evidence.Create(user, tile, url);
    }
}
