// <copyright file="Restriction.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Models
{
    public partial class Restriction : BingoRecord
    {
        public Restriction()
        {
            Tasks = new HashSet<BingoTask>();
        }

        public int RowId { get; set; }
        public string Description { get; set; } = null!;

        public virtual ICollection<BingoTask> Tasks { get; set; }
    }
}
