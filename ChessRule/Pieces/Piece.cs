namespace ChessRule
{
    // interface for a piece
    public abstract class Piece
    {
        public abstract PieceType Type { get; }
        public abstract Player Color { get; }
        public bool HasMoved { get; set; } = false; // default?

        public abstract Piece Copy();


    }
}
