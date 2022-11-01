namespace RSBingo_Framework.Models
{
    public partial class Tile : BingoRecord
    {
        public int RowId { get; set; }
        public string Name { get; set; }

        public sbyte Complete { get; set; }

        public sbyte Verfied { get; set; }

        public virtual Evidence Evidence { get; set; } = null!;
        public virtual Team Team { get; set; } = null!;
    }
}
