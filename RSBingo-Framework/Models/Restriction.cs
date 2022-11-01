using System;
using System.Collections.Generic;

namespace RSBingo_Framework.Models
{
    public partial class Restriction : BingoRecord
    {
        public Restriction()
        {
            Tasks = new HashSet<BingoTask>();
        }

        public int RowId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<BingoTask> Tasks { get; set; }
    }
}
