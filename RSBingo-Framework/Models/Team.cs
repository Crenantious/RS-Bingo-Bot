// <copyright file="Team.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Models;

public partial class Team : BingoRecord
{
    public Team()
    {
        Tiles = new HashSet<Tile>();
        Users = new HashSet<User>();
    }

    public int RowId { get; set; }
    public string Name { get; set; } = null!;
    public ulong CategoryChannelId { get; set; }
    public ulong BoardChannelId { get; set; }
    public ulong GeneralChannelId { get; set; }
    public ulong EvidenceChannelId { get; set; }
    public ulong VoiceChannelId { get; set; }
    public ulong BoardMessageId { get; set; }
    public ulong RoleId { get; set; }
    public string? Code { get; set; }

    public virtual ICollection<Tile> Tiles { get; set; }
    public virtual ICollection<User> Users { get; set; }
}
