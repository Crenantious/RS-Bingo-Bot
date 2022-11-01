using System;
using System.Collections.Generic;

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
        public long BoardChannelId { get; set; }

        public virtual Team Team { get; set; } = null!;
        public virtual ICollection<Evidence> Evidence { get; set; }
    }
}
