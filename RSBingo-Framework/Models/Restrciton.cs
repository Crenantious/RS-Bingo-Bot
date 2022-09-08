using System;
using System.Collections.Generic;

namespace RSBingo_Framework.Models
{
    public partial class Restrciton : BingoRecord
    {
        public int RowId { get; set; }
        public string? Description { get; set; }
    }
}
