namespace RSBingo_Framework.Models
{
    public partial class Team : BingoRecord
    {
        public Team()
        {
            Users = new HashSet<User>();
            Tiles = new HashSet<Tile>();
        }

        public int RowId { get; set; }

        public string Name { get; set; } = null!;

        public ulong ChannelID { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Tile> Tiles { get; set; }
    }
}
