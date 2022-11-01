

namespace RSBingo_Framework.Records
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using static RSBingo_Framework.Repository.EvidenceRepository;

    public static class TeamRecord
    {
        public static Team CreateTeam(IDataWorker dataWorker, string name, ulong boardChannelId) =>
            dataWorker.Teams.Create(name, boardChannelId);

        public static bool IsBoardVerfied(this Team team) =>
            team.Tiles.FirstOrDefault(t => t.IsNotVerified()) == null;
    }
}
