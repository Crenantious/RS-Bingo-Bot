// <copyright file="BingoTask.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Models
{
    using System;
    using System.Collections.Generic;
    using static RSBingo_Framework.Records.BingoTaskRecord;

    public partial class BingoTask : BingoRecord
    {
        public BingoTask()
        {
            Tiles = new HashSet<Tile>();
            Restrictions = new HashSet<Restriction>();
        }

        public int RowId { get; set; }
        public string Name { get; set; } = null!;
        public sbyte Difficulty { get; set; }

        public virtual ICollection<Tile> Tiles { get; set; }

        public virtual ICollection<Restriction> Restrictions { get; set; }
    }
}
