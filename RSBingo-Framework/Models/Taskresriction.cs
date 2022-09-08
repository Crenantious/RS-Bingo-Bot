using System;
using System.Collections.Generic;

namespace RSBingo_Framework.Models
{
    public partial class Taskresriction : BingoRecord
    {
        public int TaskId { get; set; }
        public int RestrictionId { get; set; }

        public virtual Restrciton Restriction { get; set; } = null!;
        public virtual BingoTask Task { get; set; } = null!;
    }
}
