namespace RSBingo_Framework.Models
{
    public partial class Tile : BingoRecord
    {
        public Tile()
        {
            Evidence = new HashSet<Evidence>();
        }

        public int RowId { get; set; }
        public string Name { get; set; }

        public sbyte Complete { get; set; }

        public sbyte Verfied { get; set; }

        public virtual Team Team { get; set; } = null!;
        public virtual ICollection<Evidence> Evidence { get; set; } = null!;
    }
}
