using System;
using System.Collections.Generic;

namespace RSBingo_Framework.Models
{
    public partial class Evidence : BingoRecord
    {
        public int Rowid { get; set; }
        public sbyte Verified { get; set; }
        public string? LocationUrl { get; set; }
        public sbyte Type { get; set; }
        public int TileId { get; set; }
        public int UserId { get; set; }

        public virtual Tile Row { get; set; } = null!;
        public virtual User RowNavigation { get; set; } = null!;
    }
}
