using System;
using System.Collections.Generic;

namespace RSBingo_Framework.Models
{
    public partial class User : BingoRecord 
    {
        public int RowId { get; set; }
        public int TeamId { get; set; }

        public virtual Team Team { get; set; } = null!;
        public virtual Evidence Evidence { get; set; } = null!;
    }
}
