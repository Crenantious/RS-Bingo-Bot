namespace RSBingo_Framework.Models
{
    public partial class Team : BingoRecord
    {
        public Team()
        {
            Users = new HashSet<User>();
        }

        public int RowId { get; set; }

        public string TeamName { get; set; } = null!;

        public string ChannelID { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Tile> Tiles { get; set; }
    }
}
