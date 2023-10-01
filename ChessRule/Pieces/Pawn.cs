namespace ChessRule
{
    public class Pawn : Piece   // Implements Piece
    {
        public override PieceType Type => PieceType.Pawn;
        public override Player Color { get; }

        // Constructor
        public Pawn(Player color)
        {
            Color = color;
        }

        // Technically a clone method
        public override Piece Copy()
        {
            Pawn copy = new Pawn(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

    }
}
