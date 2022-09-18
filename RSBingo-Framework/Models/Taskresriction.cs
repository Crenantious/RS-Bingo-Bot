// <copyright file="Taskresriction.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Models
{
    public partial class TaskRestrciton : BingoRecord
    {
        public int TaskId { get; set; }
        public int RestrictionId { get; set; }

        public virtual Restrciton Restriction { get; set; } = null!;
        public virtual BingoTask Task { get; set; } = null!;
    }
}
