using System;
using System.Collections.Generic;

namespace RSBingo_Framework.Models
{
    public partial class BingoTask : BingoRecord
    {
        public BingoTask()
        {
            Tiles = new HashSet<Tile>();
            Restrictions = new HashSet<Restriction>();
        }

        public int RowId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Tile> Tiles { get; set; }

        public virtual ICollection<Restriction> Restrictions { get; set; }
    }
}
