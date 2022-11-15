// <copyright file="BingoTaskRestriction.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Models
{
    using System;
    using System.Collections.Generic;

    public partial class BingoTaskRestriction : BingoRecord
    {
        public BingoTaskRestriction()
        {
            Tasks = new HashSet<Task>();
            Restrictions = new HashSet<Restriction>();
        }

        public int TaskId { get; set; }
        public int RestrictionId { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }

        public virtual ICollection<Restriction> Restrictions { get; set; }
    }
}
