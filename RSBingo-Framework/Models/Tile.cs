

namespace RSBingo_Framework.Models
{
    public partial class Tile : BingoRecord
    {
        public int RowId { get; set; }
        public int TeamId { get; set; }
        public int TaskId { get; set; }
        public sbyte Verfied { get; set; }

        public virtual BingoTask Task { get; set; } = null!;
        public virtual Team Team { get; set; } = null!;
    }
}
