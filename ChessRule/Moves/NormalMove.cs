namespace ChessRule
{
    public class NormalMove : Move
    {
        // All moves that simply moves a piece from one position to another
        // Move may capture a piece, but otherwise no other side effects

        public override MoveType Type => MoveType.Normal;
        public override Position FromPos { get; }
        public override Position ToPos { get; }

        public NormalMove(Position from, Position to) 
        {
            FromPos = from;
            ToPos = to;
        }

        // Execute method, makes move happen, takes piece at from and places at to.
        // Does not have in mind the legality of move, just executes it.
        // Legality logic done elsewhere
        public override void Execute(Board board)
        {
            Piece piece = board[FromPos];
            board[ToPos] = piece;
            board[FromPos] = null;
            piece.HasMoved = true;
        }
    }
}
