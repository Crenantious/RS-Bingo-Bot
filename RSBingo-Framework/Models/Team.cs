using System;
using System.Collections.Generic;

namespace RSBingo_Framework.Models
{
    public partial class Team : BingoRecord
    {
        public Team()
        {
            Evidences = new HashSet<Evidence>();
            Tiles = new HashSet<Tile>();
            Users = new HashSet<User>();
        }

        public int RowId { get; set; }
        public string Name { get; set; } = null!;
        public ulong BoardChannelId { get; set; }

        public virtual ICollection<Evidence> Evidences { get; set; }
        public virtual ICollection<Tile> Tiles { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
