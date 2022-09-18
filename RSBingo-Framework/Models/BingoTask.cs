namespace RSBingo_Framework.Models
{
    public partial class BingoTask : BingoRecord
    {
        public int RowId { get; set; }

        public string Name { get; set; } = null!;

        public sbyte Difficulty { get; set; }

        public byte[] Image { get; set; }
    }
}
