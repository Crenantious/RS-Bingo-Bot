﻿// <copyright file="Tile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Models
{
    public partial class Tile : BingoRecord
    {
        public Tile()
        {
            Evidence = new HashSet<Evidence>();
        }

        public int RowId { get; set; }
        public int TeamId { get; set; }
        public int TaskId { get; set; }
        public sbyte IsVerified { get; set; }
        public int BoardIndex { get; set; }
        public sbyte IsComplete { get; set; }

        public virtual BingoTask Task { get; set; } = null!;
        public virtual Team Team { get; set; } = null!;
        public virtual ICollection<Evidence> Evidence { get; set; }
    }
}
