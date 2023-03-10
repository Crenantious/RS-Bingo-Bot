// <copyright file="Evidence.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Models
{
    public partial class Evidence : BingoRecord
    {
        public int RowId { get; set; }
        public int TileId { get; set; }
        public ulong DiscordUserId { get; set; }
        public ulong DiscordMessageId { get; set; }
        public string Url { get; set; } = null!;
        public sbyte Status { get; set; }
        public sbyte EvidenceType { get; set; }

        public virtual User DiscordUser { get; set; } = null!;
        public virtual Tile Tile { get; set; } = null!;
    }
}
