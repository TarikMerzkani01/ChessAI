namespace ChessRule
{
    public class Knight : Piece
    {
        public override PieceType Type => PieceType.Knight;
        public override Player Color { get; }

        public Knight(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            Knight copy = new Knight(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        // Only piece that can jump

        // Helper method potentialToPos
        private static IEnumerable<Position> PotentialToPositions(Position from)
        {
            // Vertical direction loop
            foreach (Direction vDir in new Direction[] { Direction.North, Direction.South })
            {
                // Horizontal direction loop
                foreach (Direction hDir in new Direction[] { Direction.West, Direction.East })
                {
                    // Doing 1v and 2h, and 2v and 1h for all directions
                    // we get all 8 possible positions the Knight can land on
                    yield return from + 2 * vDir + hDir;
                    yield return from + vDir + 2 * hDir;
                }
            }
        }

        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            // returns potential positions if inside board and
            // if it is empty there or capturable piece is there (opposite color)
            return PotentialToPositions(from).Where(pos => Board.IsInside(pos) 
                && (board.IsEmpty(pos) || board[pos].Color != Color));
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositions(from, board).Select(to => new NormalMove(from, to));
        }


    }
}
