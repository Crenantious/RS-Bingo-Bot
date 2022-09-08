using System;
using System.Collections.Generic;

namespace RSBingo_Framework.Models
{
    public partial class Tile : BingoRecord
    {
        public int RowId { get; set; }
        public sbyte Complete { get; set; }
        public sbyte Verfied { get; set; }

        public virtual Evidence Evidence { get; set; } = null!;
    }
}
