// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Models
{
    public partial class User : BingoRecord
    {
        public User()
        {
            Evidence = new HashSet<Evidence>();
        }

        public ulong DiscordUserId { get; set; }
        public int TeamId { get; set; }

        public virtual Team Team { get; set; } = null!;
        public virtual ICollection<Evidence> Evidence { get; set; } = null!;
    }
}
