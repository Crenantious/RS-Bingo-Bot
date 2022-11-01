using System;
using System.Collections.Generic;

namespace RSBingo_Framework.Models
{
    public partial class Evidence : BingoRecord
    {
        public int RowId { get; set; }
        public int TileId { get; set; }
        public long DiscordUserId { get; set; }
        public string Url { get; set; } = null!;
        public sbyte Status { get; set; }
        public sbyte Type { get; set; }

        public virtual User DiscordUser { get; set; } = null!;
        public virtual Tile Tile { get; set; } = null!;
    }
}
