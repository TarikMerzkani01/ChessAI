using System.Security.AccessControl;

namespace ChessRule
{
    // interface for a piece
    public abstract class Piece
    {
        public abstract PieceType Type { get; }
        public abstract Player Color { get; }
        public bool HasMoved { get; set; } = false; // default?

        public abstract Piece Copy();

        // Returns a collection of all the legal moves the piece can make from
        // a position from and in a given board. Pieces don't have Position information
        // in them, hence taken as parameter here
        public abstract IEnumerable<Move> GetMoves(Position from, Board board);
        

        // Helper method, returns all reachable positions in a given direction
        // Helpful for Queen, Bishop, and Rook
        protected IEnumerable<Position> MovePositionsInDir(Position from, Board board, Direction dir)
        {
            // For loop, start at from + 1 step in dir, continues while is inside,
            // after an iteration takes another step
            // Easier now that we overloaded operators for Position and Directions
            for (Position pos = from + dir; Board.IsInside(pos); pos += dir)
            {
                if (board.IsEmpty(pos))
                {
                    yield return pos;
                    continue;
                }
                // Otherwise, there is a piece in this position
                Piece piece = board[pos];

                if (piece.Color != Color) // If opponent's piece
                {
                    yield return pos; // Position is reachable
                }
                yield break; // If player's piece, then stop here
            }
        }

        // Method that takes an array of directions

        protected IEnumerable<Position> MovePositionsInDirs(Position from, Board board, Direction[] dirs)
        {
            return dirs.SelectMany(dir => MovePositionsInDir(from, board, dir));
            // SelectMnay projects each element of a sequence to an IEnumerable<T>,
            // then it flattens the resulting sequences into one sequence.
        }

        public virtual bool CanCaptureOpponentKing(Position from, Board board)
        {
            // Returns true iff the piece can capture the Opponent's king
            // Never happens but it is used for detecting check
            return GetMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPos];
                return piece != null && piece.Type == PieceType.King; //returns move if move captures a KING
                // No need to check color, since GetMoves never returns move captures on pieces of same color
            });

        }
    }
}
